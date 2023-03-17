using System.Globalization;
using System.Text.RegularExpressions;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct Unit : IEquatable<Unit> {
    public static readonly Unit Mil = new(0);
    public static readonly Unit Millimeter = new(1);
    public static readonly Unit Inch = new(2);
    public static readonly Unit Centimeter = new(3);
    public static readonly Unit DotsPerPixelDefault = new(4);
    public static readonly Unit Meter = new(5);

    private static readonly UnitMetadata[] _metadata = new[] {
        new UnitMetadata(Mil, "Mils", "mil", "#####0.0##mil", false, MilsToCoordinate, CoordinateToMils),
        new UnitMetadata(Millimeter, "Millimeters", "mm", "#####0.0##mm", true, MillimetersToCoordinate, CoordinateToMillimeters),
        new UnitMetadata(Inch, "Inches", "in", "#####0.00#in", false, InchesToCoordinate, CoordinateToInches),
        new UnitMetadata(Centimeter, "Centimeters", "cm", "#####0.0##cm", true, CentimetersToCoordinate, CoordinateToCentimeters),
        new UnitMetadata(DotsPerPixelDefault, "Dxp Defaults", "", "#####0.###", false, DotsPerPixelToCoordinate, CoordinateToDotsPerPixel),
        new UnitMetadata(Meter, "Meters", "m", "#####0.0##m", true, MetersToCoordinate, CoordinateToMeters),
    };

    private readonly int _value;

    private Unit(int value) => _value = value;

    public static Unit FromInt32(int value) => new(value);

    public int ToInt32() => _value;

    public static explicit operator Unit(int value) => FromInt32(value);

    public static explicit operator int(Unit unit) => unit.ToInt32();

    private static bool TestIsUnitValue(string input, string suffix) =>
        Regex.IsMatch(input, $@"^\s*[+-]?\s*\d+\.?\d*\s*{suffix}\s*$");

    public static bool TryParseStringToCoordinateUnit(string input, out Coordinate result, out Unit unit) {
        result = default;
        unit = default;

        input = input?.Trim() ?? "";
        foreach (var m in _metadata) {
            if (TestIsUnitValue(input, m.Suffix)) {
                if (TryParseStringToDouble(input[.. ^ m.Suffix.Length], out var value)) {
                    unit = m.Instance;
                    result = m.UnitValueToCoordinate(value);
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }

    public static bool TryParseStringToCoordinateUnit(string input, out Coordinate result) {
        result = default;

        input = input?.Trim() ?? "";
        foreach (var m in _metadata) {
            if (TestIsUnitValue(input, m.Suffix)) {
                if (TryParseStringToDouble(input[.. ^ m.Suffix.Length], out var value)) {
                    result = m.UnitValueToCoordinate(value);
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }

    public static Coordinate StringToCoordinateUnit(string input, out Unit unit) =>
        TryParseStringToCoordinateUnit(input, out var result, out unit)
            ? result
            : throw new FormatException($"Invalid coordinate: {input}");

    public static Coordinate StringToCoordUnit(string input) =>
        TryParseStringToCoordinateUnit(input, out var result)
            ? result
            : throw new FormatException($"Invalid coordinate: {input}");

    public static string CoordUnitToString(Coordinate coordinate, Unit unit, Coordinate grid) {
        if (unit._value < 0 || unit._value >= _metadata.Length) {
            throw new ArgumentException("Unsupported unit", nameof(unit));
        }

        Coordinate gridSnappedCoord = (int) (Math.Round((double) coordinate / grid) * grid);
        var m = _metadata[unit._value];
        var value = m.CoordinateToUnitValue(gridSnappedCoord);
        return value.ToString(m.Format, CultureInfo.InvariantCulture);
    }

    public override bool Equals(object? obj) => obj is Unit other && Equals(other);

    public bool Equals(Unit other) => _value == other._value;

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(Unit left, Unit right) => left.Equals(right);

    public static bool operator !=(Unit left, Unit right) => !left.Equals(right);
}
