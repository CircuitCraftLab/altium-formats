using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbMetaTrack : PcbUnknown {
    public IList<CoordinatePoint> Vertices { get; }

    public Coordinate Width { get; set; }

    public PcbMetaTrack() : base(PcbPrimitiveObjectId.None) {
        Vertices = new List<CoordinatePoint>();
        Width = Coordinate.FromMils(10);
    }

    public PcbMetaTrack(params CoordinatePoint[] vertices) : this() {
        Vertices = vertices;
    }

    public override CoordinateRectangular CalculateBounds() =>
        CoordinateRectangular.Empty;

    public IEnumerable<Tuple<CoordinatePoint, CoordinatePoint>> Lines =>
        Vertices.Zip(Vertices.Skip(1), Tuple.Create);
}
