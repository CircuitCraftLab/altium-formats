using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public abstract class PcbPrimitive : Primitive {
    public virtual PcbPrimitiveDisplayInfo GetDisplayInfo() => new();

    public abstract PcbPrimitiveObjectId ObjectId { get; }

    public Layer Layer { get; set; }

    public PcbFlags Flags { get; set; }

    public bool IsLocked {
        get => !Flags.HasFlag(PcbFlags.Unlocked);
        set => Flags = Flags.WithFlag(PcbFlags.Unlocked, !value);
    }

    public bool IsTentingTop {
        get => Flags.HasFlag(PcbFlags.TentingTop);
        set => Flags = Flags.WithFlag(PcbFlags.TentingTop, value);
    }

    public bool IsTentingBottom {
        get => Flags.HasFlag(PcbFlags.TentingBottom);
        set => Flags = Flags.WithFlag(PcbFlags.TentingBottom, value);
    }

    public bool IsKeepOut {
        get => Flags.HasFlag(PcbFlags.KeepOut);
        set => Flags = Flags.WithFlag(PcbFlags.KeepOut, value);
    }

    public bool IsFabricationTop {
        get => Flags.HasFlag(PcbFlags.FabricationTop);
        set => Flags = Flags.WithFlag(PcbFlags.FabricationTop, value);
    }

    public bool IsFabricationBottom {
        get => Flags.HasFlag(PcbFlags.FabricationBottom);
        set => Flags = Flags.WithFlag(PcbFlags.FabricationBottom, value);
    }

    public string UniqueId { get; set; } = string.Empty!;

    public override CoordinateRectangular CalculateBounds() => CoordinateRectangular.Empty;

    protected PcbPrimitive() {
        Layer = LayerMetadata.Get("TopLayer").Id;
        Flags = PcbFlags.Unlocked | PcbFlags.Unknown8;
    }
}
