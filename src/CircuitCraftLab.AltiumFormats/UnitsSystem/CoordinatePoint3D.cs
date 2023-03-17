namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct CoordinatePoint3D : IEquatable<CoordinatePoint3D> {
    public static readonly CoordinatePoint3D Zero = new();

    public Coordinate X { get; }

    public Coordinate Y { get; }

    public Coordinate Z { get; }

    public CoordinatePoint3D(Coordinate x, Coordinate y, Coordinate z) => (X, Y, Z) = (x, y, z);

    public void Deconstruct(out Coordinate x, out Coordinate y, out Coordinate z) => (x, y, z) = (X, Y, Z);

    public static CoordinatePoint3D FromMils(double milsX, double milsY, double milsZ) =>
        new(Coordinate.FromMils(milsX), Coordinate.FromMils(milsY), Coordinate.FromMils(milsZ));

    public static CoordinatePoint3D FromMillimeters(double mmsX, double mmsY, double mmsZ) =>
        new(Coordinate.FromMillimeters(mmsX), Coordinate.FromMillimeters(mmsY), Coordinate.FromMillimeters(mmsZ));

    public override string ToString() => $"X:{X} Y:{Y} Z:{Z}";

    public string ToString(Unit unit) => $"X:{X.ToString(unit)} Y:{Y.ToString(unit)} Z:{Z.ToString(unit)}";

    public string ToString(Unit unit, Coordinate grid) => $"X:{X.ToString(unit, grid)} Y:{Y.ToString(unit, grid)} Z:{Z.ToString(unit, grid)}";

    public override bool Equals(object? obj) => obj is CoordinatePoint3D other && Equals(other);

    public bool Equals(CoordinatePoint3D other) => X == other.X && Y == other.Y && Z == other.Z;

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();

    public static bool operator ==(CoordinatePoint3D left, CoordinatePoint3D right) => left.Equals(right);

    public static bool operator !=(CoordinatePoint3D left, CoordinatePoint3D right) => !(left == right);
}
