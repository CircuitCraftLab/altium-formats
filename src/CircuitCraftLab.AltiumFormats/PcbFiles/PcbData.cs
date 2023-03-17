using Avalonia.Controls;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public abstract class PcbData {
    internal string FileName { get; private set; } = string.Empty!;

    public Dictionary<string, Image> EmbeddedImages { get; } = new Dictionary<string, Image>();
}

public abstract class PcbData<THeader, TItem> : PcbData
    where THeader : new()
    where TItem : IContainer, new() {
    public THeader Header { get; } = new THeader();

    public List<TItem> Items { get; } = new List<TItem>();

    protected PcbData() {
    }
}
