using System.IO.Compression;
using System.Text;

using OpenMcdf;

using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.CompoundFiles;

public abstract class CompoundFileReader<TData> : IDisposable where TData : new() {
    private readonly List<string> _context;

    protected Dictionary<string, string> SectionKeys { get; } = new Dictionary<string, string>();

    internal CompoundFile CompoundFile { get; private set; } = null!;

    protected TData? Data { get; private set; }

    public List<string> Warnings { get; }

    public List<string> Errors { get; }

    public string Context => string.Join(":", _context);

    public CompoundFileReader() {
        _context = new List<string>();
        Warnings = new List<string>();
        Errors = new List<string>();
    }

    protected abstract void DoRead();

    private void Clear() {
        Data = new TData();
        Warnings.Clear();
        Errors.Clear();
    }

    public TData? Read(string fileName) {
        Clear();
        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (CompoundFile = new CompoundFile(stream)) {
            DoRead();
        }
        CompoundFile = null!;
        return Data;
    }

    public TData? Read(Stream stream) {
        Clear();
        using (CompoundFile = new CompoundFile(stream)) {
            DoRead();
        }
        CompoundFile = null!;
        return Data;
    }

    protected string GetSectionKeyFromRefName(string refName) {
        if (SectionKeys.TryGetValue(refName, out var result)) {
            return result;
        } else {
            return refName[..(refName.Length > 31
                ? 31
                : refName.Length)]
                .Replace('/', '_');
        }
    }

    protected void BeginContext(string context) =>
        _context.Add(context);

    protected void EndContext() =>
        _context.RemoveAt(_context.Count - 1);

    protected void EmitError(string message) {
        Console.Error.WriteLine($"Error: {message}");
        Errors.Add($"{Context}\t{message}");
        throw new InvalidDataException(message);
    }

    protected void EmitWarning(string message) {
        Console.Error.WriteLine($"Warning: {message}");
        Warnings.Add($"{Context}\t{message}");
    }

    protected void AssertValue<T>(string name, T actual, params T[] expected) where T : IEquatable<T> {
        if (!expected.Any(s => EqualityComparer<T>.Default.Equals(s, actual))) {
            EmitError($"Expected {name ?? "value"} to be {string.Join(", ", expected)}, actual value is {actual}");
        }
    }

    protected bool CheckValue<T>(string name, T actual, params T[] expected) where T : IEquatable<T> {
        if (!expected.Any(s => EqualityComparer<T>.Default.Equals(s, actual))) {
            EmitWarning($"Expected {name ?? "value"} to be {string.Join(", ", expected)}, actual value is {actual}");
            return false;
        } else {
            return true;
        }
    }

    internal static uint ReadHeader(CFStorage storage) {
        using var header = storage.GetStream("Header").GetBinaryReader();
        return header.ReadUInt32();
    }

    internal static byte[] ReadBlock(BinaryReader reader, int emptySize = 0) {
        return ReadBlock(reader, reader.ReadBytes, emptySize: emptySize);
    }

    internal static void ReadBlock(BinaryReader reader, Action<int> interpreter, Action onEmpty = null!, int emptySize = 0) {
        ReadBlock<object>(reader, size => {
            interpreter(size);
            return null!;
        }, () => {
            onEmpty?.Invoke();
            return null!;
        }, emptySize);
    }

    internal static T ReadBlock<T>(BinaryReader reader, Func<int, T> interpreter, Func<T> onEmpty = null!, int emptySize = 0) {
        var size = reader.ReadInt32();
        var sanitizedSize = size & 0x00ffffff;
        if (size > emptySize) {
            var position = reader.BaseStream.Position;
            try {
                var result = interpreter(size);
                if (reader.BaseStream.Position > position + sanitizedSize) {
                    throw new IndexOutOfRangeException("Read past the end of the block");
                }
                return result;
            } finally {
                reader.BaseStream.Position = position + sanitizedSize;
            }
        } else if (onEmpty != null) {
            return onEmpty();
        } else {
            return default!;
        }
    }

    internal static T ParseCompressedZlibData<T>(byte[] data, Func<MemoryStream, T> interpreter) {
        const int ZlibHeaderSize = 2;
        using var compressedData = new MemoryStream(data, ZlibHeaderSize, data.Length - ZlibHeaderSize);
        using var decompressedData = new MemoryStream();
        using var deflater = new DeflateStream(compressedData, CompressionMode.Decompress);
        deflater.CopyTo(decompressedData);
        decompressedData.Position = 0;
        return interpreter.Invoke(decompressedData);
    }

    internal (string id, T data) ReadCompressedStorage<T>(BinaryReader reader, Func<MemoryStream, T> interpreter) {
        return ReadBlock(reader, size => {
            if (reader.ReadByte() != 0xD0) {
                EmitError("Expected 0xD0 tag");
            }

            var id = ReadPascalShortString(reader);
            var zlibData = ReadBlock(reader);
            return ParseCompressedZlibData(zlibData, stream => (id, interpreter(stream)));
        });
    }

    internal (string id, byte[] data) ReadCompressedStorage(BinaryReader reader) {
        return ReadCompressedStorage(reader, s => s.ToArray());
    }

    internal static string ParseRawString(byte[] data, int index = 0, int size = -1, Encoding encoding = null!) {
        size = size == -1
            ? data.Length
            : size;
        if (size != 0) {
            encoding ??= Encoding.GetEncoding(1252);
            return encoding.GetString(data, index, size);
        } else {
            return "";
        }
    }

    internal static string ReadRawString(BinaryReader reader, int size, Encoding encoding = null!) {
        var data = reader.ReadBytes(size);
        return ParseRawString(data, encoding: encoding);
    }

    internal static string ParseCString(byte[] data, Encoding encoding = null!) {
        return ParseRawString(data, 0, data.Length - 1, encoding);
    }

    internal static string ReadCString(BinaryReader reader, int size, Encoding encoding = null!) {
        var data = reader.ReadBytes(size);
        return ParseCString(data, encoding);
    }

    internal static string ReadStringFontName(BinaryReader reader) {
        var pos = reader.BaseStream.Position;
        var data = new List<byte>();
        ushort unicodeChar;
        while (data.Count < 32 && (unicodeChar = reader.ReadUInt16()) != 0) {
            data.AddRange(BitConverter.GetBytes(unicodeChar));
        }
        reader.BaseStream.Position = pos + 32;
        return ParseRawString(data.ToArray(), encoding: Encoding.Unicode);
    }

    internal static string ReadPascalString(BinaryReader reader, Encoding encoding = null!) {
        return ReadBlock(reader, size => ReadCString(reader, size, encoding));
    }

    internal static string ReadPascalShortString(BinaryReader reader, Encoding encoding = null!) {
        return ReadRawString(reader, reader.ReadByte(), encoding: encoding);
    }

    internal static string ReadStringBlock(BinaryReader reader, Encoding encoding = null!) {
        return ReadBlock(reader, size => ReadPascalShortString(reader, encoding));
    }

    internal static ParameterCollection ReadParameters(BinaryReader reader, int size, bool raw = false, Encoding encoding = null!) {
        var data = raw
            ? ReadRawString(reader, size, encoding)
            : ReadCString(reader, size, encoding);
        return ParameterCollection.FromString(data);
    }

    internal static CoordinatePoint ReadCoordPoint(BinaryReader reader) {
        var x = reader.ReadInt32();
        var y = reader.ReadInt32();
        return new CoordinatePoint(x, y);
    }

    internal List<string> ReadWideStrings(CFStorage storage) {
        BeginContext("WideStrings");

        var result = new List<string>();
        using (var reader = storage.GetStream("WideStrings").GetBinaryReader()) {
            var parameters = ReadBlock(reader, size => ReadParameters(reader, size));
            for (var i = 0; i < parameters.Count; ++i) {
                var chars = parameters[$"ENCODEDTEXT{i}"]
                    .AsIntList()
                    .Select(codepoint => Convert.ToChar(codepoint));
                var text = string.Concat(chars);
                result.Add(text);
            }
        }

        EndContext();

        return result;
    }

    protected static byte[] ExtractStreamData(BinaryReader reader, long startPosition, long endPosition) {
        if (reader == null) {
            throw new ArgumentNullException(nameof(reader));
        }

        if (!reader.BaseStream.CanSeek) {
            throw new InvalidOperationException("Reader base stream is not seekable");
        }

        var currentPosition = reader.BaseStream.Position;
        try {
            var count = endPosition - startPosition;
            reader.BaseStream.Position = startPosition;
            return reader.ReadBytes((int) count);
        } finally {
            reader.BaseStream.Position = currentPosition;
        }
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                CompoundFile?.Close();
                CompoundFile = null!;
            }

            disposedValue = true;
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
