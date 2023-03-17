using System.Globalization;
using System.Text.RegularExpressions;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public static class UnitsConverter {
    public const double InternalUnits = 10000.0;

    public static double MilsToMillimeters(double mils) =>
        mils * 0.0254;

    public static double MillimetersToMils(double millimeters) =>
        millimeters / 0.0254;

    public static double CoordinateToMils(Coordinate coordinate) =>
        (coordinate / InternalUnits);

    public static Coordinate MilsToCoordinate(double mils) =>
        (int) (mils * InternalUnits);

    public static double CoordinateToMillimeters(Coordinate coord) =>
        MilsToMillimeters(CoordinateToMils(coord));

    public static Coordinate MillimetersToCoordinate(double millimeters) =>
        MilsToCoordinate(MillimetersToMils(millimeters));

    public static double CoordinateToCentimeters(Coordinate coordinate) =>
        CoordinateToMillimeters(coordinate) * 0.1;

    public static Coordinate CentimetersToCoordinate(double centimeters) =>
        MillimetersToCoordinate(centimeters * 10.0);

    public static double CoordinateToInches(Coordinate coordinate) =>
        CoordinateToMils(coordinate) * 0.001;

    public static Coordinate InchesToCoordinate(double inches) =>
        MilsToCoordinate(inches * 1000.0);

    public static double CoordinateToDotsPerPixel(Coordinate coordinate) =>
        CoordinateToMils(coordinate) / 10.0;

    public static Coordinate DotsPerPixelToCoordinate(double value) =>
        MilsToCoordinate(value * 10.0);

    public static double DotsPerPixelFractionToMils(int number, int fraction) =>
        number * 10.0 + fraction / 10000.0;

    public static (int number, int fraction) MilsToDotsPerPixelFraction(double mils) =>
        ((int) mils / 10, (int) Math.Round((mils / 10.0 - Math.Truncate(mils / 10.0)) * 100000));

    public static (int number, int fraction) CoordinateToDotsPerPixelFraction(Coordinate coordinate) =>
        MilsToDotsPerPixelFraction(CoordinateToMils(coordinate));

    public static Coordinate DotsPerPixelFractionToCoordinate(int number, int fraction) =>
        MilsToCoordinate(DotsPerPixelFractionToMils(number, fraction));

    public static Coordinate MetersToCoordinate(double meters) =>
        MillimetersToCoordinate(meters * 1000.0);

    public static double CoordinateToMeters(Coordinate coordinate) =>
        CoordinateToMillimeters(coordinate) * 0.001;

    public static Coordinate StringToCoordinateUnit(string input, out Unit unit) =>
        Unit.StringToCoordinateUnit(input, out unit);

    public static bool TryParseStringToCoordinateUnit(string input, out Coordinate result, out Unit unit) =>
        Unit.TryParseStringToCoordinateUnit(input, out result, out unit);

    public static string CoordinateUnitToString(Coordinate coordinate, Unit unit) =>
        CoordinateUnitToString(coordinate, unit, 1);

    public static string CoordinateUnitToString(Coordinate coordinate, Unit unit, Coordinate grid) =>
        Unit.CoordUnitToString(coordinate, unit, grid);

    public static string LayerToString(Layer layer) => LayerMetadata.GetName(layer);

    public static Layer StringToLayer(string layer) => LayerMetadata.Get(layer ?? "").Id;

    public static string UnitToString(Unit unit) => unit.ToString()!;

    internal static double StringToDouble(string input) =>
        double.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);

    public static double NormalizeAngle(double degrees) =>
        (degrees > 0 && degrees % 360.0 == 0)
            ? 360.0
            : (degrees % 360.0 + 360.0) % 360.0;

    internal static bool TryParseStringToDouble(string input, out double value) =>
        double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value);

    internal static string Format(string format, params object[] args) =>
        string.Format(CultureInfo.InvariantCulture, format, args);

    internal static ref CoordinatePoint[] TranslatePoints(ref CoordinatePoint[] points,
        in CoordinatePoint value) {
        if (value == CoordinatePoint.Zero) {
            return ref points;
        }

        for (var i = 0; i < points.Length; ++i) {
            var p = points[i];
            points[i] = new CoordinatePoint(p.X + value.X, p.Y + value.Y);
        }

        return ref points;
    }

    internal static ref CoordinatePoint[] RotatePoints(ref CoordinatePoint[] points,
        in CoordinatePoint anchor, double angleDegrees) {
        var angleRadians = -angleDegrees * Math.PI / 180.0;
        var cosAngle = Math.Cos(angleRadians);
        var sinAngle = Math.Sin(angleRadians);
        for (var i = 0; i < points.Length; ++i) {
            var (x, y) = points[i];
            double localX = x - anchor.X;
            double localY = y - anchor.Y;
            var rotatedX = localX * cosAngle + localY * sinAngle;
            var rotatedY = localY * cosAngle - localX * sinAngle;
            points[i] = new CoordinatePoint(anchor.X + (int) rotatedX, anchor.Y + (int) rotatedY);
        }
        return ref points;
    }

    private static readonly Random generator = new();

    public static string GenerateUniqueId() {
        var result = new char[8];
        for (var i = 0; i < 8; ++i) {
            result[i] = (char) generator.Next('A', 'Z');
        }
        return new string(result);
    }

    private static readonly Regex _designatorParser = new(@"^(?<Prefix>.*?)(?<Number>\d*)\s*$");

    public static string GenerateDesignator(IEnumerable<string> existingDesignators) {
        var largestDesignator = existingDesignators
            .Select(s => _designatorParser.Match(s ?? ""))
            .Select(m => (m.Groups["Prefix"]?.Value ?? "",
                int.TryParse(m.Groups["Number"]?.Value ?? "", out var n)
                    ? n
                    : (int?) null))
            .OrderBy(pn => pn)
            .LastOrDefault();
        if (largestDesignator.Item2 != null) {
            return $"{largestDesignator.Item1}{largestDesignator.Item2 + 1}";
        } else if (largestDesignator.Item1 != null) {
            return largestDesignator.Item1;
        } else {
            return "1";
        }
    }
}
