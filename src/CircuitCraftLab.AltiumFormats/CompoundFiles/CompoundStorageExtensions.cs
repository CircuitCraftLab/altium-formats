using OpenMcdf;

namespace CircuitCraftLab.AltiumFormats.CompoundFiles;

public static class CompoundStorageExtensions {
    public static CFStream GetOrAddStream(this CFStorage storage, string streamName) {
        if (storage == null) {
            throw new ArgumentNullException(nameof(storage));
        }

        return storage.TryGetStream(streamName, out var childStream)
            ? childStream
            : storage.AddStream(streamName);
    }

    public static CFStorage GetOrAddStorage(this CFStorage storage, string storageName) {
        if (storage == null) {
            throw new ArgumentNullException(nameof(storage));
        }

        return storage.TryGetStorage(storageName, out var childStorage)
            ? childStorage
            : storage.AddStorage(storageName);
    }
}
