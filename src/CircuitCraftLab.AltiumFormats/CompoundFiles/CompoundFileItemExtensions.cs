using OpenMcdf;

namespace CircuitCraftLab.AltiumFormats.CompoundFiles;

public static class CompoundFileItemExtensions {
    public static CFItem TryGetChild(this CFItem item, string name) {
        if (item is CFStorage storage) {
            if (storage.TryGetStorage(name, out var resultStorage)) {
                return resultStorage;
            } else if (storage.TryGetStream(name, out var resultStream)) {
                return resultStream;
            }
        }
        return null!;
    }

    public static CFItem GetChild(this CFItem item, string name) {
        if (item == null) {
            throw new ArgumentNullException(nameof(item));
        }

        if (item is CFStorage storage) {
            return TryGetChild(item, name) ?? throw new ArgumentException($"Item '{name}' doesn't exists within storage '{storage.Name}'.", nameof(name));
        } else {
            throw new InvalidOperationException($"Item '{item.Name}' is a stream and cannot have child items.");
        }
    }

    public static IEnumerable<CFItem> Children(this CFItem item) {
        if (item == null) {
            throw new ArgumentNullException(nameof(item));
        }

        var result = new List<CFItem>();
        if (item is CFStorage storage) {
            storage.VisitEntries(childItem => result.Add(childItem), false);
        } else {
            throw new InvalidOperationException($"Item '{item.Name}' is a stream and cannot have child items.");
        }
        return result;
    }
}
