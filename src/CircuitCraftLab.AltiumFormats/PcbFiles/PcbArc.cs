using CircuitCraftLab.AltiumFormats.UnitsSystem;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbArc : PcbPrimitive {
    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.Arc;

    public CoordinatePoint Location { get; set; }

    public Coordinate Radius { get; set; }

    public double StartAngle { get; set; }

    public double EndAngle { get; set; }

    public Coordinate Width { get; set; }

    public PcbArc() : base() {
        Radius = DotsPerPixelFractionToCoordinate(10, 0);
        StartAngle = 0;
        EndAngle = 360;
    }

    public override CoordinateRectangular CalculateBounds() =>
        new(Location.X - Radius, Location.Y - Radius, Radius * 2, Radius * 2);
}
