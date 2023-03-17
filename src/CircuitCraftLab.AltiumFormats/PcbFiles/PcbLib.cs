using System.Collections;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbLib : PcbData<PcbLibHeader, PcbComponent>, IEnumerable<PcbComponent> {
    public string UniqueId { get; internal set; } = string.Empty!;

    public PcbLib() : base() {
    }

    public void Add(PcbComponent component) {
        if (component == null) return;

        if (string.IsNullOrEmpty(component.Pattern)) {
            component.Pattern = $"Component_{Items.Count + 1}";
        }

        Items.Add(component);
    }

    IEnumerator<PcbComponent> IEnumerable<PcbComponent>.GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
