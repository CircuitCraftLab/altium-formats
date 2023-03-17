using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbTrack : PcbPrimitive {
    public override PcbPrimitiveDisplayInfo GetDisplayInfo() =>
        new("", Width, null);

    public override PcbPrimitiveObjectId ObjectId =>
        PcbPrimitiveObjectId.Track;

    public CoordinatePoint Start { get; set; }

    public CoordinatePoint End { get; set; }

    public Coordinate Width { get; set; }

    public PcbTrack() : base() =>
        Width = Coordinate.FromMils(10);

    public override CoordinateRectangular CalculateBounds() =>
        new(Start, End);
}
