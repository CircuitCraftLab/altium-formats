using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct Coordinate : IEquatable<Coordinate>, IComparable<Coordinate> {
    public static readonly Coordinate OneInch = MilsToCoordinate(1000);

    private readonly int _value;

    private Coordinate(int value) => _value = value;

    public static Coordinate FromMils(double mils) => MilsToCoordinate(mils);

    public double ToMils() => CoordinateToMils(this);

    public static Coordinate FromMillimeters(double millimeters) => MillimetersToCoordinate(millimeters);

    public double ToMillimeters() => CoordinateToMillimeters(this);

    public static Coordinate FromInt32(int value) => new(value);

    public int ToInt32() => _value;

    public override string ToString() => CoordinateUnitToString(_value, Unit.Mil);

    public string ToString(Unit unit) => CoordinateUnitToString(_value, unit);

    public string ToString(Unit unit, Coordinate grid) => CoordinateUnitToString(_value, unit, grid);

    public static implicit operator Coordinate(int value) => FromInt32(value);

    public static implicit operator int(Coordinate coord) => coord._value;

    public override bool Equals(object? obj) => obj is Coordinate other && Equals(other);

    public bool Equals(Coordinate other) => _value == other._value;

    public int CompareTo(Coordinate other) => _value < other._value ? -1 : _value > other._value ? 1 : 0;

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(Coordinate left, Coordinate right) => left.Equals(right);

    public static bool operator !=(Coordinate left, Coordinate right) => !left.Equals(right);

    public static bool operator <(Coordinate left, Coordinate right) => left.CompareTo(right) < 0;

    public static bool operator <=(Coordinate left, Coordinate right) => left.CompareTo(right) <= 0;

    public static bool operator >(Coordinate left, Coordinate right) => left.CompareTo(right) > 0;

    public static bool operator >=(Coordinate left, Coordinate right) => left.CompareTo(right) >= 0;
}
