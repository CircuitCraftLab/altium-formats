namespace CircuitCraftLab.AltiumFormats;

public interface IComponent : IContainer {
    string Name { get; }

    string Description { get; }
}
