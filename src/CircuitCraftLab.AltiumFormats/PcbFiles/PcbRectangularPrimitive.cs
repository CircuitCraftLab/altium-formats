using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public abstract class PcbRectangularPrimitive : PcbPrimitive {
    public CoordinatePoint Corner1 { get; set; }

    public CoordinatePoint Corner2 { get; set; }

    public double Rotation { get; set; }

    public Coordinate Width {
        get => Corner2.X - Corner1.X;
        set => Corner2 = new CoordinatePoint(Corner1.X + value, Corner2.Y);
    }

    public Coordinate Height {
        get => Corner2.Y - Corner1.Y;
        set => Corner2 = new CoordinatePoint(Corner1.X, Corner1.Y + value);
    }

    public override CoordinateRectangular CalculateBounds() =>
        CoordinateRectangular.FromRotatedRect(new CoordinateRectangular(Corner1, Corner2),
            Corner1, Rotation);
}
