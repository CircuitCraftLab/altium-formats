using System.Globalization;
using System.Text;

using Avalonia.Media;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct ParameterValue : IEquatable<ParameterValue> {
    private const NumberStyles _numberStyles = NumberStyles.Any;

    private static readonly IFormatProvider _formatProvider = CultureInfo.InvariantCulture;

    internal const string TrueValueShort = "T";
    internal const string TrueValueLong = "TRUE";
    internal const string FalseValueShort = "F";
    internal const string FalseValueLong = "FALSE";

    internal static readonly string[] TrueValues = new string[] { TrueValueShort, TrueValueLong };
    internal static readonly string[] FalseValues = new string[] { FalseValueShort, FalseValueLong };

    internal static readonly char[] ListSeparators = new[] { ',', '?' };

    private readonly string _data;
    private readonly int _level;

    private char ListSeparator => ListSeparators.ElementAtOrDefault(_level);

    internal ParameterValue(string data, int level) =>
        (_data, _level) = (data, level);

    internal bool IsAscii() => _data == null || Encoding.UTF8.GetByteCount(_data) == _data.Length;

    internal string AsUtf8Data() => Encoding.GetEncoding(1252).GetString(Encoding.UTF8.GetBytes(_data));

    public override string ToString() => AsString();

    public string AsString() => _data;

    public string? AsStringOrDefault(string? defaultValue = default) =>
        AsString() ?? defaultValue;

    public int AsInt() => int.Parse(_data, _formatProvider);

    public int AsIntOrDefault(int defaultValue = default) =>
        int.TryParse(_data, NumberStyles.Integer, _formatProvider, out var result)
            ? result
            : defaultValue;

    public T AsEnumOrDefault<T>(T defaultValue = default!) where T : Enum =>
        (T) Enum.ToObject(typeof(T), AsIntOrDefault(Convert.ToInt32(defaultValue, CultureInfo.InvariantCulture)));

    public double AsDouble() => double.Parse(_data, _formatProvider);

    public double AsDoubleOrDefault(double defaultValue = default) =>
        double.TryParse(_data, NumberStyles.Any, _formatProvider, out var result)
            ? result
            : defaultValue;

    public bool AsBool() {
        if (TrueValues.Contains(_data)) {
            return true;
        } else if (FalseValues.Contains(_data) || _data == null) {
            return false;
        } else {
            throw new FormatException("Value is not a valid boolean");
        }
    }

    public Coordinate AsCoordinate() => StringToCoordinateUnit(_data, out _);

    public Color AsColor() => Color.FromUInt32((uint) AsInt());

    public Color AsColorOrDefault() => Color.FromUInt32((uint) AsIntOrDefault());

    public ParameterCollection AsParameters() => new(_data, _level + 1);

    public IEnumerable<ParameterValue> AsEnumerable(char? separator = null) {
        if (string.IsNullOrEmpty(_data)) yield break;

        foreach (var item in _data.Split(separator ?? ListSeparator)) {
            yield return new ParameterValue(item, '\0');
        }
    }

    public IReadOnlyList<ParameterValue> AsList(char? separator = null) =>
        AsEnumerable(separator)
            .ToArray();

    public IReadOnlyList<string> AsStringList(char? separator = null) =>
        AsEnumerable(separator)
            .Select(p => p.AsString())
            .ToArray();

    public IReadOnlyList<int> AsIntList(char? separator = null) =>
        AsEnumerable(separator)
            .Select(p => p.AsInt())
            .ToArray();

    public IReadOnlyList<double> AsDoubleList(char? separator = null) =>
        AsEnumerable(separator)
            .Select(p => p.AsDouble())
            .ToArray();

    public IReadOnlyList<Coordinate> AsCoordinateList(char? separator = null) =>
        AsEnumerable(separator)
            .Select(p => p.AsCoordinate())
            .ToArray();

    public bool IsList(char? separator = null) =>
        _data.Contains(separator ?? ListSeparator, StringComparison.InvariantCulture);

    public bool IsParameters() =>
        _data.Contains(ParameterCollection.EntrySeparators.ElementAtOrDefault(_level + 1), StringComparison.InvariantCulture);

    public override bool Equals(object? obj) => obj is ParameterValue other && Equals(other);

    public bool Equals(ParameterValue other) => _data == other._data;

    public override int GetHashCode() => _data.GetHashCode(StringComparison.InvariantCulture);

    public static bool operator ==(ParameterValue left, ParameterValue right) => left.Equals(right);

    public static bool operator !=(ParameterValue left, ParameterValue right) => !(left == right);
}
