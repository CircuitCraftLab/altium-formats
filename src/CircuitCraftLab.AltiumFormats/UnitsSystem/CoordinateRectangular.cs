using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct CoordinateRectangular : IEquatable<CoordinateRectangular> {
    public static readonly CoordinateRectangular Empty = new();

    public CoordinatePoint Location1 { get; }

    public CoordinatePoint Location2 { get; }

    public Coordinate Width => Location2.X - Location1.X;

    public Coordinate Height => Location2.Y - Location1.Y;

    public bool IsEmpty => Width == 0 && Height == 0;

    public CoordinatePoint Center =>
        new((Location1.X + Location2.X) / 2, (Location1.Y + Location2.Y) / 2);

    public CoordinateRectangular(CoordinatePoint loc1, CoordinatePoint loc2) {
        Location1 = new CoordinatePoint(Math.Min(loc1.X, loc2.X), Math.Min(loc1.Y, loc2.Y));
        Location2 = new CoordinatePoint(Math.Max(loc1.X, loc2.X), Math.Max(loc1.Y, loc2.Y));
    }

    public CoordinateRectangular(Coordinate x, Coordinate y, Coordinate w, Coordinate h) :
        this(new CoordinatePoint(x, y), new CoordinatePoint(x + w, y + h)) {
    }

    public void Deconstruct(out CoordinatePoint location1, out CoordinatePoint location2) =>
        (location1, location2) = (Location1, Location2);

    public void Deconstruct(out Coordinate x, out Coordinate y, out Coordinate w, out Coordinate h) =>
        (x, y, w, h) = (Location1.X, Location1.Y, Width, Height);

    public bool Contains(in CoordinatePoint point) =>
        Location1.X <= point.X && point.X <= Location2.X &&
        Location1.Y <= point.Y && point.Y <= Location2.Y;

    public bool Intersects(in CoordinateRectangular other) =>
        Location1.X <= other.Location2.X && Location2.X >= other.Location1.X &&
        Location1.Y <= other.Location2.Y && Location2.Y >= other.Location1.Y;

    public CoordinatePoint[] GetPoints() {
        return new[] {
            Location1, new CoordinatePoint(Location2.X, Location1.Y),
            Location2, new CoordinatePoint(Location1.X, Location2.Y)
        };
    }

    public CoordinatePoint[] RotatedPoints(CoordinatePoint anchorPoint, double rotationDegrees) {
        var points = GetPoints();
        return RotatePoints(ref points, anchorPoint, rotationDegrees);
    }

    public override string ToString() => $"({Location1} {Location2})";

    public static CoordinateRectangular FromRotatedRect(in CoordinateRectangular coordRect,
        in CoordinatePoint anchorPoint, double rotationDegrees) {
        var points = coordRect.RotatedPoints(anchorPoint, rotationDegrees);
        return new CoordinateRectangular(new CoordinatePoint(points.Min(p => p.X), points.Min(p => p.Y)),
            new CoordinatePoint(points.Max(p => p.X), points.Max(p => p.Y)));
    }

    public static CoordinateRectangular Union(in CoordinateRectangular left, in CoordinateRectangular right) {
        if (left.IsEmpty) {
            return right;
        } else if (right.IsEmpty) {
            return left;
        } else {
            return new CoordinateRectangular(
                new CoordinatePoint(Math.Min(left.Location1.X, right.Location1.X),
                    Math.Min(left.Location1.Y, right.Location1.Y)),
                new CoordinatePoint(Math.Max(left.Location2.X, right.Location2.X),
                    Math.Max(left.Location2.Y, right.Location2.Y)));
        }
    }

    public static CoordinateRectangular Union(IEnumerable<CoordinateRectangular> collection) =>
        collection.Aggregate(Empty, (acc, rect) => Union(acc, rect));

    public override bool Equals(object? obj) => obj is CoordinateRectangular other && Equals(other);

    public bool Equals(CoordinateRectangular other) => Location1 == other.Location1 && Location2 == other.Location2;

    public override int GetHashCode() => Location1.GetHashCode() ^ Location2.GetHashCode();

    public static bool operator ==(CoordinateRectangular left, CoordinateRectangular right) => left.Equals(right);

    public static bool operator !=(CoordinateRectangular left, CoordinateRectangular right) => !(left == right);
}
