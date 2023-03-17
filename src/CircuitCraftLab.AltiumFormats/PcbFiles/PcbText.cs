using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbText : PcbRectangularPrimitive {
    public override PcbPrimitiveDisplayInfo GetDisplayInfo() =>
        new(Text, null, null);

    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.Text;

    public bool Mirrored { get; set; }

    public PcbTextKind TextKind { get; set; }

    public PcbTextStrokeFont StrokeFont { get; set; }

    public Coordinate StrokeWidth { get; set; }

    public bool FontBold { get; set; }

    public bool FontItalic { get; set; }

    public string FontName { get; set; }

    public Coordinate BarcodeLRMargin { get; set; }

    public Coordinate BarcodeTBMargin { get; set; }

    public bool FontInverted { get; set; }

    public Coordinate FontInvertedBorder { get; set; }

    public bool FontInvertedRect { get; set; }

    public Coordinate FontInvertedRectWidth { get; set; }

    public Coordinate FontInvertedRectHeight { get; set; }

    public PcbTextJustification FontInvertedRectJustification { get; set; }

    public Coordinate FontInvertedRectTextOffset { get; set; }

    public string Text { get; set; }

    internal int WideStringsIndex { get; set; }

    public PcbText() : base() {
        Text = "String";
        Height = Coordinate.FromMils(60);
        TextKind = PcbTextKind.Stroke;
        StrokeFont = PcbTextStrokeFont.SansSerif;
        StrokeWidth = Coordinate.FromMils(10);
        FontName = "Arial";
        FontInvertedBorder = Coordinate.FromMils(20);
        FontInvertedRectJustification = PcbTextJustification.MiddleCenter;
        FontInvertedRectTextOffset = Coordinate.FromMils(2);
    }

    internal CoordinateRectangular CalculateRectangular(bool useAbsolutePosition) {
        var w = (TextKind == PcbTextKind.Stroke)
            ? (Text.Length * Height * 12) / 13
            : (Text.Length * Height / 2);
        var h = Height;
        var x = Mirrored ? -w : 0;
        var y = 0;
        if (useAbsolutePosition) {
            x += Corner1.X;
            y += Corner1.Y;
        }
        return new CoordinateRectangular(x, y, w, h);
    }

    public override CoordinateRectangular CalculateBounds() =>
        CoordinateRectangular.FromRotatedRect(CalculateRectangular(true), Corner1, Rotation);
}
