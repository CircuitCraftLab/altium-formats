using System.Text;

using OpenMcdf;

namespace CircuitCraftLab.AltiumFormats.CompoundFiles;

public static class CompoundStreamExtensions {
    public static MemoryStream GetMemoryStream(this CFStream stream) {
        if (stream == null) {
            throw new ArgumentNullException(nameof(stream));
        }
        return new MemoryStream(stream.GetData());
    }

    public static BinaryReader GetBinaryReader(this CFStream stream, Encoding encoding) =>
        new(stream.GetMemoryStream(), encoding, false);

    public static BinaryReader GetBinaryReader(this CFStream stream) =>
        GetBinaryReader(stream, Encoding.UTF8);

    public static void Write(this CFStream stream, Action<BinaryWriter> action, Encoding encoding) {
        using var memoryStream = new MemoryStream();
        using var writer = new BinaryWriter(memoryStream, encoding);
        action?.Invoke(writer);
        stream.SetData(memoryStream.ToArray());
    }

    public static void Write(this CFStream stream, Action<BinaryWriter> action) =>
        stream.Write(action, Encoding.UTF8);
}
