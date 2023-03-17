using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.UnitsSystem;

public readonly struct Parameter : IEquatable<Parameter> {
    internal const string Utf8Prefix = "%UTF8%";

    public string Name { get; }

    public ParameterValue Value { get; }

    internal Parameter(string name, string value, int level) =>
        (Name, Value) = (name, new ParameterValue(value, level));

    public override string ToString() =>
        Value.IsAscii()
            ? $"{Name}={Value}"
            : $"{Utf8Prefix}{Name}={Value.AsUtf8Data()}|||{Name}={Value}";

    public string ToUnicodeString() => $"{Name}={Value}";

    public override bool Equals(object? obj) => obj is Parameter other && Equals(other);

    public bool Equals(Parameter other) => Name == other.Name && Value == other.Value;

    public override int GetHashCode() => Name.GetHashCode(StringComparison.InvariantCultureIgnoreCase);

    public static bool operator ==(Parameter left, Parameter right) => left.Equals(right);

    public static bool operator !=(Parameter left, Parameter right) => !(left == right);
}
