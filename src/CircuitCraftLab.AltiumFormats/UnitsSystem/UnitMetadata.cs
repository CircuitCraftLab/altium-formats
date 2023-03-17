namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

internal class UnitMetadata {
    public Unit Instance { get; }

    public string Name { get; }

    public string Suffix { get; }

    public string Format { get; }

    public bool IsMetric { get; }

    public Func<double, Coordinate> UnitValueToCoordinate { get; }

    public Func<Coordinate, double> CoordinateToUnitValue { get; }

    public UnitMetadata(Unit instance, string name, string suffix, string format, bool isMetric,
        Func<double, Coordinate> unitValueToCoordinate,
        Func<Coordinate, double> coordinateToUnitValue) {
        Instance = instance;
        Name = name;
        Suffix = suffix;
        Format = format;
        IsMetric = isMetric;
        UnitValueToCoordinate = unitValueToCoordinate;
        CoordinateToUnitValue = coordinateToUnitValue;
    }
}
