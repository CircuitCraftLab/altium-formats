namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct CoordinatePoint : IEquatable<CoordinatePoint> {
    public static readonly CoordinatePoint Zero = new();

    public Coordinate X { get; }

    public Coordinate Y { get; }

    public CoordinatePoint(Coordinate x, Coordinate y) => (X, Y) = (x, y);

    public void Deconstruct(out Coordinate x, out Coordinate y) => (x, y) = (X, Y);

    public static CoordinatePoint FromMils(double milsX, double milsY) =>
        new(Coordinate.FromMils(milsX), Coordinate.FromMils(milsY));

    public static CoordinatePoint FromMillimeters(double millimetersX, double millimetersY) =>
        new(Coordinate.FromMillimeters(millimetersX), Coordinate.FromMillimeters(millimetersY));

    public override string ToString() => $"X:{X} Y:{Y}";

    public string ToString(Unit unit) => $"X:{X.ToString(unit)} Y:{Y.ToString(unit)}";

    public string ToString(Unit unit, Coordinate grid) => $"X:{X.ToString(unit, grid)} Y:{Y.ToString(unit, grid)}";

    public override bool Equals(object? obj) => obj is CoordinatePoint other && Equals(other);

    public bool Equals(CoordinatePoint other) => X == other.X && Y == other.Y;

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

    public static bool operator ==(CoordinatePoint left, CoordinatePoint right) => left.Equals(right);

    public static bool operator !=(CoordinatePoint left, CoordinatePoint right) => !(left == right);
}
