using CircuitCraftLab.AltiumFormats.UnitsSystem;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbPad : PcbPrimitive {
    public override PcbPrimitiveDisplayInfo GetDisplayInfo() =>
        new(Designator, Math.Max(SizeTop.X, SizeBottom.X), Math.Max(SizeTop.Y, SizeBottom.Y));

    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.Pad;

    public string Designator { get; set; } = string.Empty!;

    public CoordinatePoint Location { get; set; }

    public double Rotation { get; set; }

    public bool IsPlated { get; set; }

    public int JumperId { get; set; }

    private PcbStackMode _stackMode;

    public PcbStackMode StackMode {
        get => _stackMode;
        set {
            _stackMode = value;
            switch (value) {
                case PcbStackMode.Simple:
                    SizeMiddle = SizeTop;
                    SizeBottom = SizeTop;
                    ShapeMiddle = ShapeTop;
                    ShapeBottom = ShapeTop;
                    CornerRadiusMid = CornerRadiusTop;
                    CornerRadiusBot = CornerRadiusTop;
                    break;
                case PcbStackMode.TopMiddleBottom:
                    SizeMiddle = SizeMiddle;
                    ShapeMiddle = ShapeMiddle;
                    CornerRadiusMid = CornerRadiusMid;
                    break;
                default:
                    break;
            }
        }
    }

    public CoordinatePoint Size {
        get => SizeTop;
        set => SetSizeAll(value);
    }

    public PcbPadShape Shape {
        get => ShapeTop;
        set => SetShapeAll(value);
    }

    public byte CornerRadius {
        get => CornerRadiusTop;
        set => SetCornerRadiusAll(value);
    }

    public CoordinatePoint SizeTop {
        get => SizeLayers[0];
        set => SetSizeTop(value);
    }

    public PcbPadShape ShapeTop {
        get => ShapeLayers[0];
        set => SetShapeTop(value);
    }

    public byte CornerRadiusTop {
        get => ShapeTop == PcbPadShape.RoundedRectangle
            ? CornerRadiusPercentage[0]
            : (byte) 0;
        set => SetCornerRadiusTop(value);
    }

    public CoordinatePoint SizeMiddle {
        get => SizeLayers[1];
        set => SetSizeMiddle(value);
    }

    public PcbPadShape ShapeMiddle {
        get => ShapeLayers[1];
        set => SetShapeMiddle(value);
    }

    public byte CornerRadiusMid {
        get => ShapeMiddle == PcbPadShape.RoundedRectangle
            ? CornerRadiusPercentage[1]
            : (byte) 0;
        set => SetCornerRadiusMiddle(value);
    }

    public CoordinatePoint SizeBottom {
        get => SizeLayers[31];
        set => SetSizeBottom(value);
    }

    public PcbPadShape ShapeBottom {
        get => ShapeLayers[31];
        set => SetShapeBottom(value);
    }

    public byte CornerRadiusBot {
        get => ShapeBottom == PcbPadShape.RoundedRectangle
            ? CornerRadiusPercentage[31]
            : (byte) 0;
        set => SetCornerRadiusBottom(value);
    }

    public CoordinatePoint OffsetFromHoleCenter {
        get => OffsetsFromHoleCenter[0];
        set => OffsetsFromHoleCenter[0] = value;
    }

    public Coordinate HoleSize { get; set; }

    public PcbPadHoleShape HoleShape { get; set; }

    public double HoleRotation { get; set; }

    public Coordinate HoleSlotLength { get; set; }

    public bool PasteMaskExpansionManual { get; set; }

    public Coordinate PasteMaskExpansion { get; set; }

    public bool SolderMaskExpansionManual { get; set; }

    public Coordinate SolderMaskExpansion { get; set; }

    public IList<CoordinatePoint> SizeMiddleLayers =>
        SizeLayers.Skip(2).Take(SizeLayers.Count - 3).ToArray();

    public IList<PcbPadShape> ShapeMiddleLayers =>
        ShapeLayers.Skip(2).Take(ShapeLayers.Count - 3).ToArray();

    public IList<CoordinatePoint> OffsetsFromHoleCenter { get; } = new CoordinatePoint[32];

    public IList<CoordinatePoint> SizeLayers { get; } = new CoordinatePoint[32];

    public IList<PcbPadShape> ShapeLayers { get; } = new PcbPadShape[32];

    public IList<byte> CornerRadiusPercentage { get; } = new byte[32];

    public bool HasHole =>
        Layer == LayerMetadata.Get("MultiLayer").Id;

    internal bool HasRoundedRectangles =>
        ShapeLayers.Any(s => s == PcbPadShape.RoundedRectangle);

    internal bool NeedsFullStackData =>
        (StackMode == PcbStackMode.FullStack) || HasRoundedRectangles ||
        OffsetsFromHoleCenter.Any(o => o != CoordinatePoint.Zero);

    public PcbPad(PcbPadTemplate template = PcbPadTemplate.Tht) : base() {
        switch (template) {
            case PcbPadTemplate.Tht:
                Layer = LayerMetadata.Get("MultiLayer").Id;
                break;
            case PcbPadTemplate.SmtTop:
                Layer = LayerMetadata.Get("TopLayer").Id;
                break;
            case PcbPadTemplate.SmtBottom:
                Layer = LayerMetadata.Get("BottomLayer").Id;
                break;
        }
        IsPlated = true;
        StackMode = PcbStackMode.Simple;

        var defaultSize = CoordinatePoint.FromMils(60, 60);
        var defaultShape = PcbPadShape.Round;
        byte defaultRadiusPercentage = 50;
        HoleSize = Coordinate.FromMils(30);
        HoleShape = PcbPadHoleShape.Round;

        for (var i = 0; i < SizeLayers.Count; ++i) {
            SizeLayers[i] = defaultSize;
        }
        for (var i = 0; i < ShapeLayers.Count; ++i) {
            ShapeLayers[i] = defaultShape;
        }
        for (var i = 0; i < CornerRadiusPercentage.Count; ++i) {
            CornerRadiusPercentage[i] = defaultRadiusPercentage;
        }
    }

    internal CoordinateRectangular CalculatePartRect(PcbPadPart part, bool useAbsolutePosition) {
        var solderMaskExpansion = SolderMaskExpansionManual
            ? SolderMaskExpansion
            : MilsToCoordinate(8);
        var solderMaskExpansionTop = IsTentingTop
            ? MilsToCoordinate(0)
            : solderMaskExpansion;
        var solderMaskExpansionBottom = IsTentingBottom 
            ? MilsToCoordinate(0)
            : solderMaskExpansion;

        Coordinate width, height;
        var offset = OffsetFromHoleCenter;
        switch (part) {
            case PcbPadPart.TopLayer:
                width = SizeTop.X;
                height = SizeTop.Y;
                break;
            case PcbPadPart.BottomLayer:
                width = SizeBottom.X;
                height = SizeBottom.Y;
                break;
            case PcbPadPart.TopSolder:
                width = SizeTop.X + solderMaskExpansionTop;
                height = SizeTop.Y + solderMaskExpansionTop;
                break;
            case PcbPadPart.BottomSolder:
                width = SizeBottom.X + solderMaskExpansionBottom;
                height = SizeBottom.Y + solderMaskExpansionBottom;
                break;
            case PcbPadPart.Hole:
                width = PcbPadHoleShape.Slot == HoleShape ? HoleSlotLength : HoleSize;
                height = HoleSize;
                offset = CoordinatePoint.Zero;
                break;
            default:
                return CoordinateRectangular.Empty;
        }

        Coordinate x = offset.X - width / 2;
        Coordinate y = offset.Y - height / 2;
        if (useAbsolutePosition) {
            x += Location.X;
            y += Location.Y;
        }
        return new CoordinateRectangular(x, y, width, height);
    }

    internal CoordinateRectangular CalculatePartBounds(PcbPadPart part) =>
        CoordinateRectangular.FromRotatedRect(CalculatePartRect(part, true), Location,
            Rotation + (part == PcbPadPart.Hole ? HoleRotation : 0));

    public override CoordinateRectangular CalculateBounds() {
        var result = CalculatePartBounds(PcbPadPart.BottomSolder);
        result = CoordinateRectangular.Union(result, CalculatePartBounds(PcbPadPart.TopSolder));
        result = CoordinateRectangular.Union(result, CalculatePartBounds(PcbPadPart.BottomLayer));
        result = CoordinateRectangular.Union(result, CalculatePartBounds(PcbPadPart.TopLayer));
        if (HasHole) {
            result = CoordinateRectangular.Union(result, CalculatePartBounds(PcbPadPart.Hole));
        }
        return result;
    }

    private void SetSizeAll(CoordinatePoint value) {
        for (var i = 0; i < SizeLayers.Count; ++i) {
            SizeLayers[i] = value;
        }
    }

    private void SetSizeTop(CoordinatePoint value) {
        if (StackMode == PcbStackMode.Simple) {
            SetSizeAll(value);
        } else {
            SizeLayers[0] = value;
        }
    }

    private void SetSizeMiddle(CoordinatePoint value) {
        for (var i = 1; i < SizeLayers.Count - 1; ++i) {
            SizeLayers[i] = value;
        }
    }

    private void SetSizeBottom(CoordinatePoint value) {
        if (StackMode == PcbStackMode.Simple) {
            SetSizeAll(value);
        } else {
            SizeLayers[SizeLayers.Count - 1] = value;
        }
    }

    private void SetShapeAll(PcbPadShape value) {
        for (var i = 0; i < ShapeLayers.Count; ++i) {
            ShapeLayers[i] = value;
        }
    }

    private void SetShapeTop(PcbPadShape value) {
        if (StackMode == PcbStackMode.Simple) {
            SetShapeAll(value);
        } else {
            ShapeLayers[0] = value;
        }
    }

    private void SetShapeMiddle(PcbPadShape value) {
        if (StackMode == PcbStackMode.Simple) {
            SetShapeAll(value);
        } else {
            for (var i = 1; i < ShapeLayers.Count - 1; ++i) {
                ShapeLayers[i] = value;
            }
        }
    }

    private void SetShapeBottom(PcbPadShape value) {
        if (StackMode == PcbStackMode.Simple) {
            SetShapeAll(value);
        } else {
            ShapeLayers[ShapeLayers.Count - 1] = value;
        }
    }

    private void SetCornerRadiusAll(byte value) {
        for (var i = 0; i < CornerRadiusPercentage.Count; ++i) {
            CornerRadiusPercentage[i] = value;
        }
    }

    private void SetCornerRadiusTop(byte value) {
        if (StackMode == PcbStackMode.Simple) {
            SetCornerRadiusAll(value);
        } else {
            CornerRadiusPercentage[0] = value;
        }
    }

    private void SetCornerRadiusMiddle(byte value) {
        if (StackMode == PcbStackMode.Simple) {
            SetCornerRadiusAll(value);
        } else {
            for (var i = 1; i < CornerRadiusPercentage.Count - 1; ++i) {
                CornerRadiusPercentage[i] = value;
            }
        }
    }

    private void SetCornerRadiusBottom(byte value) {
        if (StackMode == PcbStackMode.Simple) {
            SetCornerRadiusAll(value);
        } else {
            CornerRadiusPercentage[CornerRadiusPercentage.Count - 1] = value;
        }
    }
}
