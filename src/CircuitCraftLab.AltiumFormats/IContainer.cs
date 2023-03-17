using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats;

public interface IContainer {
    IEnumerable<T> GetPrimitivesOfType<T>(bool flatten = true) where T : Primitive;

    CoordinateRectangular CalculateBounds();
}
