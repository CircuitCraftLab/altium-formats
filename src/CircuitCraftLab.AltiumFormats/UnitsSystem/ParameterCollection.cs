using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

[DebuggerDisplay("Count = {Count}")]
public class ParameterCollection : IEnumerable<(string key, ParameterValue value)> {
    internal static readonly char[] EntrySeparators = new[] { '|', '`' };

    private const char _keyValueSeparator = '=';
    private const char _listSeparator = ',';

    public string Data { get; private set; } = string.Empty!;

    public int Level { get; private set; }

    public bool UseLongBooleans { get; set; }

    private string _record = string.Empty!;
    private string _bookmark = string.Empty!;

    private readonly List<string> _keys;
    private readonly Dictionary<string, Parameter> _parameters;

    private char EntrySeparator => EntrySeparators.ElementAtOrDefault(Level);

    public ParameterCollection() {
        _keys = new List<string>();
        _parameters = new Dictionary<string, Parameter>();
    }

    internal ParameterCollection(string data, int level = 0) {
        _keys = new List<string>();
        _parameters = new Dictionary<string, Parameter>();
        Data = data;
        Level = level;
        ParseData();
    }

    private void ParseData() {
        var ignored = new HashSet<string>();

        var sepKeyValue = new char[] { _keyValueSeparator };

        var entries = Data.Split(new char[] { EntrySeparator }, StringSplitOptions.RemoveEmptyEntries)
            .Select((line, index) => (index, line.Split(sepKeyValue, 2)));
        foreach (var (i, entryKeyValue) in entries) {
            var key = (entryKeyValue.Length > 1)
                ? entryKeyValue[0] :
                "";
            var value = entryKeyValue
                .Last()
                .TrimEnd('\r', '\n');
            if (ignored.Contains(key)) {
                continue;
            }

            if (key.StartsWith(Parameter.Utf8Prefix, StringComparison.InvariantCultureIgnoreCase)) {
                key = key[Parameter.Utf8Prefix.Length..];
                value = Encoding.UTF8.GetString(Encoding.GetEncoding(1252).GetBytes(value));
                ignored.Add(key);
            } else if (key.ToUpperInvariant() == "RECORD") {
                if (string.IsNullOrEmpty(_record)) {
                    _record = value;
                } else if (value != _record) {
                    throw new Exception();
                }
            }

            if (Contains(key)) {
                AddKey(key, true);
            } else {
                InternalAddData(key, value);
            }
        }
    }

    public static ParameterCollection FromString(string data) => new(data);

    private IEnumerable<Parameter> GetParametersWithValues() =>
        _keys.Where(k => _parameters.ContainsKey(k)).Select(k => _parameters[k]);

    private string InternalToString(Func<Parameter, string> parameterSerializer) {
        var separator = EntrySeparator.ToString(CultureInfo.InvariantCulture);
        var sb = new StringBuilder();
        foreach (var p in GetParametersWithValues()) {
            if (p.Name.ToUpperInvariant() == "RECORD" && sb.Length > 0) {
                sb.Append('\r');
            }
            sb.Append(separator);
            sb.Append(parameterSerializer(p));
        }
        return sb.ToString();
    }

    public override string ToString() => InternalToString(p => p.ToString());

    public string ToUnicodeString() => InternalToString(p => p.ToUnicodeString());

    private void InternalAddData(string key, string data, bool forceAddKey = false) {
        var parameterValue = new Parameter(key, data, Level);

        key = key.ToUpperInvariant();
        AddKey(key, forceAddKey);
        _parameters[key] = parameterValue;
    }

    public void AddKey(string key, bool forceAddKey = false) {
        key = key.ToUpperInvariant();
        if (forceAddKey || !_parameters.ContainsKey(key)) {
            _keys.Add(key);
        }
    }

    private void AddData<T>(string key, T value, bool ignoreDefaultValue) {
        if (value == null) {
            return;
        }

        if (!(ignoreDefaultValue && EqualityComparer<T>.Default.Equals(value, default))) {
            if (value is IConvertible convertible) {
                InternalAddData(key, convertible.ToString(CultureInfo.InvariantCulture));
            } else {
                InternalAddData(key, value.ToString()!);
            }
        } else {
            AddKey(key);
        }
    }

    public void Add(string key, string? value, bool ignoreDefaultValue = true) =>
        AddData(key, value, ignoreDefaultValue);

    public void Add(string key, int value, bool ignoreDefaultValue = true) =>
        AddData(key, value, ignoreDefaultValue);

    public void Add<T>(string key, T value, bool ignoreDefaultValue = true) where T : Enum =>
        AddData(key, Convert.ToInt32(value, CultureInfo.InvariantCulture), ignoreDefaultValue);

    public void Add(string key, double value, bool ignoreDefaultValue = true, int decimals = 6) {
        if (!ignoreDefaultValue || value != 0) {
            var format = "#########0." + string.Concat(Enumerable.Repeat("0", decimals)) + string.Concat(Enumerable.Repeat("#", 6 - decimals));
            InternalAddData(key, value.ToString(format, CultureInfo.InvariantCulture));
        } else {
            AddKey(key);
        }
    }

    public void Add(string key, bool value, bool ignoreDefaultValue = true) {
        if (!ignoreDefaultValue || value) {
            InternalAddData(key, value
                ? ParameterValue.TrueValues[UseLongBooleans ? 1 : 0]
                : ParameterValue.FalseValues[UseLongBooleans ? 1 : 0]);
        } else {
            AddKey(key);
        }
    }

    public void Add(string key, Coordinate value, bool ignoreDefaultValue = true) {
        if (!ignoreDefaultValue || (int) value != 0) {
            InternalAddData(key, value.ToMils().ToString("#####0.#####mil", CultureInfo.InvariantCulture));
        } else {
            AddKey(key);
        }
    }

    public void Add(string key, Color value, bool ignoreDefaultValue = true) =>
        AddData(key, ColorTranslator.ToWin32(value), ignoreDefaultValue);

    public void Remove(string key) {
        _parameters.Remove(key.ToUpperInvariant());
        _keys.Remove(key.ToUpperInvariant());
    }

    public bool Contains(string key) => _parameters.ContainsKey(key.ToUpperInvariant());

    public int IndexOf(string key) => _keys.IndexOf(key.ToUpperInvariant());

    public string GetKey(int index) => _keys[index];

    public void MoveKey(string key) {
        _keys.RemoveAll(k => key.ToUpperInvariant() == k);
        _keys.Add(key.ToUpperInvariant());
    }

    public void MoveKeys(string startKey, bool updateExisting = true) {
        var startIndex = IndexOf(startKey);
        if (startIndex < 0) {
            return;
        }

        for (var i = startIndex; i < KeyCount; ++i) {
            var key = _keys[i];
            AddKey(key, true);

            if (updateExisting) {
                _keys[i] = null!;
            }

            if (key == _bookmark) {
                break;
            }
        }
        if (updateExisting) {
            _keys.RemoveAll(k => k == null);
        }
    }

    public void SetBookmark() => _bookmark = _keys.LastOrDefault()!;

    public ParameterValue this[string key] {
        get {
            if (TryGetValue(key, out var result)) {
                return result.Value;
            } else {
                return new Parameter().Value;
            }
        }
    }

    public Parameter this[int index] => _parameters.TryGetValue(_keys[index], out var parameter)
        ? parameter
        : default;

    public bool TryGetValue(string key, out Parameter result) =>
        _parameters.TryGetValue(key.ToUpperInvariant(), out result);

    public ParameterValue ValueOrDefault(string key, string defaultValue = default!) {
        if (TryGetValue(key, out var value)) {
            return value.Value;
        } else {
            return new Parameter(key, defaultValue, _listSeparator).Value;
        }
    }

    public IEnumerator<(string key, ParameterValue value)> GetEnumerator() {
        foreach (var p in GetParametersWithValues()) {
            yield return (p.Name, p.Value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int KeyCount => _keys.Count;

    public int Count => _parameters.Count;
}
