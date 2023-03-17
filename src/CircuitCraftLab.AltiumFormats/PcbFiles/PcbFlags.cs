namespace CircuitCraftLab.AltiumFormats.PcbFiles;

[Flags]
public enum PcbFlags {
    None = 0,
    Unknown2 = 2,
    Unlocked = 4,
    Unknown8 = 8,
    Unknown16 = 16,
    TentingTop = 32,
    TentingBottom = 64,
    FabricationTop = 128,
    FabricationBottom = 256,
    KeepOut = 512,
}
