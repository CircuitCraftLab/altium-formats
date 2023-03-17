using System.IO.Compression;
using System.Net;
using System.Text;

using OpenMcdf;

using CircuitCraftLab.AltiumFormats.PcbFiles;
using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.CompoundFiles;

public abstract class CompoundFileWriter<TData> : IDisposable {
    internal CompoundFile? CompoundFile { get; private set; }

    protected TData? Data { get; private set; }

    public CompoundFileWriter() {
    }

    protected abstract void DoWrite(string fileName);

    public void Write(TData data, string fileName, bool overwrite = false) {
        Data = data;
        using (CompoundFile = new CompoundFile()) {
            DoWrite(fileName);
            using var stream = new FileStream(fileName, overwrite
                ? FileMode.Create
                : FileMode.CreateNew);
            CompoundFile.Save(stream);
        }
        CompoundFile = null;
    }

    public void WriteStream(TData data, Stream stream) {
        Data = data;
        using (CompoundFile = new CompoundFile()) {
            DoWrite("");
            CompoundFile.Save(stream);
        }
        CompoundFile = null;
    }

    internal static void WriteHeader(CFStorage storage, int recordCount) {
        storage.GetOrAddStream("Header").Write(writer => writer.Write(recordCount));
    }

    internal static void WriteBlock(BinaryWriter writer, byte[] data, byte flags = 0, int emptySize = 0) {
        writer.Write((flags << 24) | data.Length);
        if (data.Length > emptySize) {
            writer.Write(data ?? Array.Empty<byte>());
        }
    }

    internal static void WriteBlock(BinaryWriter writer, Action<BinaryWriter> serializer,
        byte flags = 0, int emptySize = 0) {
        var posStart = writer.BaseStream.Position;

        writer.Write(0);
        serializer?.Invoke(writer);

        var posEnd = writer.BaseStream.Position;
        writer.BaseStream.Position = posStart;

        var length = (int) (posEnd - posStart - sizeof(int));
        writer.Write((flags << 24) | length);

        if (length > emptySize) {
            writer.BaseStream.Position = posEnd;
        }
    }

    internal static byte[] CompressZlibData(byte[] data) {
        static int Adler32(byte[] buf) {
            const int mod = 65521;
            var s1 = 1;
            var s2 = 0;
            foreach (var b in buf) {
                s1 = (s1 + b) % mod;
                s2 = (s2 + s1) % mod;
            }
            return IPAddress.HostToNetworkOrder((s2 << 16) + s1);
        }

        using var compressedData = new MemoryStream();
        using var decompressedData = new MemoryStream(data);
        using var deflater = new DeflateStream(compressedData, CompressionMode.Compress, true);
        compressedData.WriteByte(0x78);
        compressedData.WriteByte(0x9c);

        decompressedData.CopyTo(deflater);
        deflater.Close();

        using (var binaryWriter = new BinaryWriter(compressedData)) {
            binaryWriter.Write(Adler32(data));
        }

        return compressedData.ToArray();
    }

    internal static void WriteCompressedStorage(BinaryWriter writer, string id, byte[] data) =>
        WriteCompressedStorage(writer, id, w => w.Write(data ?? Array.Empty<byte>()));

    internal static void WriteCompressedStorage(BinaryWriter writer, string id, Action<BinaryWriter> serializer) {
        WriteBlock(writer, w => {
            w.Write((byte) 0xD0);
            WritePascalShortString(w, id);

            using var memoryStream = new MemoryStream();
            using var binaryWriter = new BinaryWriter(memoryStream);
            serializer?.Invoke(binaryWriter);

            var data = memoryStream.ToArray();
            var zlibData = CompressZlibData(data);
            WriteBlock(w, zlibData);
        }, 0x01);
    }

    internal static byte[] SerializeRawString(string data, Encoding encoding = null!) {
        data ??= string.Empty;
        encoding ??= Encoding.GetEncoding(1252);
        return encoding.GetBytes(data);
    }

    internal static void WriteRawString(BinaryWriter writer, string data, Encoding encoding = null!) {
        var rawData = SerializeRawString(data, encoding);
        writer.Write(rawData);
    }

    internal static void WriteCString(BinaryWriter writer, string data, Encoding encoding = null!) {
        WriteRawString(writer, data, encoding);
        writer.Write((byte) 0x00);
    }

    internal static void WriteStringFontName(BinaryWriter writer, string data) {
        var rawData = SerializeRawString(data, Encoding.Unicode).Take(30).ToArray();
        writer.Write(rawData);
        for (var i = rawData.Length; i < 32; ++i) {
            writer.Write((byte) 0);
        }
    }

    internal static void WritePascalString(BinaryWriter writer, string data, Encoding encoding = null!) {
        WriteBlock(writer, w => WriteCString(w, data, encoding));
    }

    internal static void WritePascalShortString(BinaryWriter writer, string data, Encoding encoding = null!) {
        data ??= string.Empty;
        encoding ??= Encoding.GetEncoding(1252);
        writer.Write((byte) encoding.GetByteCount(data));
        WriteRawString(writer, data, encoding: encoding);
    }

    internal static void WriteStringBlock(BinaryWriter writer, string data, Encoding encoding = null!) {
        WriteBlock(writer, w => WritePascalShortString(w, data, encoding));
    }

    internal static void WriteParameters(BinaryWriter writer, ParameterCollection parameters,
        bool raw = false, Encoding encoding = null!, bool outputUtfKeys = true) {
        var data = outputUtfKeys
            ? parameters.ToString()
            : parameters.ToUnicodeString();
        if (raw) {
            WriteRawString(writer, data, encoding);
        } else {
            WriteCString(writer, data, encoding);
        }
    }

    internal static void WriteCoordPoint(BinaryWriter writer, CoordinatePoint data) {
        writer.Write(data.X);
        writer.Write(data.Y);
    }

    internal static void WriteWideStrings(CFStorage storage, PcbComponent component) {
        var texts = component.Primitives.OfType<PcbText>().ToList();
        storage.GetOrAddStream("WideStrings").Write(writer =>
        {
            var parameters = new ParameterCollection();
            for (var i = 0; i < texts.Count; ++i) {
                var text = texts[i];
                text.WideStringsIndex = i;

                var data = text.Text ?? "";
                var codepoints = data.Select(c => Convert.ToInt32(c));
                var intList = string.Join(",", codepoints);
                parameters.Add($"ENCODEDTEXT{i}", intList);
            }
            WriteBlock(writer, w => WriteParameters(w, parameters));
        });
    }

    protected string GetSectionKeyFromComponentPattern(string refName) =>
        refName[..(refName.Length > 31
            ? 31
            : refName.Length)]
            .Replace('/', '_');

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                CompoundFile?.Close();
                CompoundFile = null;
            }
            disposedValue = true;
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
