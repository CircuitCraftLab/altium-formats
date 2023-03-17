namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbUnknown : PcbPrimitive {
    private readonly PcbPrimitiveObjectId _objectId;

    public override PcbPrimitiveObjectId ObjectId => _objectId;

    public PcbUnknown(PcbPrimitiveObjectId objectId) {
        _objectId = objectId;
    }
}
