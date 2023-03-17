using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbPrimitiveDisplayInfo {
    public string Name { get; } = string.Empty!;

    public Coordinate? SizeX { get; }

    public Coordinate? SizeY { get; }

    public PcbPrimitiveDisplayInfo() {
    }

    public PcbPrimitiveDisplayInfo(string name, Coordinate? sizeX, Coordinate? sizeY) =>
        (Name, SizeX, SizeY) = (name, sizeX, sizeY);
}
