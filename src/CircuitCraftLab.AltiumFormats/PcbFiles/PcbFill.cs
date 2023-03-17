namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbFill : PcbRectangularPrimitive {
    public override PcbPrimitiveDisplayInfo GetDisplayInfo() =>
        new("", Width, Height);

    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.Fill;
}
