using System.Diagnostics;

using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats;

public abstract class Primitive {
    public IEnumerable<byte>? RawData { get; internal set; }

    public Primitive? Owner { get; internal set; }

    [Conditional("DEBUG")]
    internal void SetRawData(in byte[] rawData) => RawData = rawData;

    public abstract CoordinateRectangular CalculateBounds();

    public virtual bool IsVisible => Owner?.IsVisible ?? true;
}
