using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbRegion : PcbPrimitive {
    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.Region;

    public ParameterCollection Parameters { get; internal set; } = new ParameterCollection();

    public List<CoordinatePoint> Outline { get; } = new();

    public override CoordinateRectangular CalculateBounds() {
        return new(
            new CoordinatePoint(Outline.Min(p => p.X), Outline.Min(p => p.Y)),
            new CoordinatePoint(Outline.Max(p => p.X), Outline.Max(p => p.Y)));
    }
}
