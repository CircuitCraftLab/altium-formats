using System.Text.RegularExpressions;

using OpenMcdf;

namespace CircuitCraftLab.AltiumFormats.CompoundFiles;

public static class CompoundFileExtensions {
    private static readonly Regex _pathElementSplitter = new(@"(?<!\\)\/+", RegexOptions.Compiled);

    public static CFItem TryGetItem(this CompoundFile cf, string path) {
        if (cf == null) {
            throw new ArgumentNullException(nameof(cf));
        }

        var pathElements = _pathElementSplitter.Split(path);
        CFItem item = cf.RootStorage;
        foreach (var pathElement in pathElements) {
            item = item.TryGetChild(pathElement);
            if (item == null) break;
        }
        return item!;
    }

    public static CFItem GetItem(this CompoundFile cf, string path) {
        return TryGetItem(cf, path) ?? throw new ArgumentException($"Storage or stream with path '{path}' doesn't exist.", nameof(path));
    }

    public static CFStorage TryGetStorage(this CompoundFile cf, string path) {
        return (CFStorage) TryGetItem(cf, path);
    }

    public static CFStorage GetStorage(this CompoundFile cf, string path) {
        return TryGetStorage(cf, path) ?? throw new ArgumentException($"Storage with path '{path}' doesn't exist.", nameof(path));
    }

    public static CFStream TryGetStream(this CompoundFile cf, string path) {
        return (CFStream) TryGetItem(cf, path);
    }

    public static CFStream GetStream(this CompoundFile cf, string path) {
        return TryGetStream(cf, path) ?? throw new ArgumentException($"Stream with path '{path}' doesn't exist.", nameof(path));
    }
}
