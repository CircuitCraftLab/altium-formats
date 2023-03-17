using CircuitCraftLab.AltiumFormats.UnitsSystem;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbVia : PcbPrimitive {
    public override PcbPrimitiveDisplayInfo GetDisplayInfo() =>
        new($"{FromLayer} to {ToLayer}", Diameter, null);

    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.Via;

    public CoordinatePoint Location { get; set; }

    public Coordinate HoleSize { get; set; }

    public Layer FromLayer { get; set; }

    public Layer ToLayer { get; set; }

    public Coordinate ThermalReliefAirGapWidth { get; set; }

    public int ThermalReliefConductors { get; set; }

    public Coordinate ThermalReliefConductorsWidth { get; set; }

    public bool SolderMaskExpansionManual { get; set; }

    public Coordinate SolderMaskExpansion { get; set; }

    public PcbStackMode DiameterStackMode { get; set; }

    public IList<Coordinate> Diameters { get; } = new Coordinate[32];

    public Coordinate Diameter {
        get => Diameters[Diameters.Count - 1];
        set => SetDiameterAll(value);
    }

    public Coordinate DiameterTop {
        get => DiameterStackMode == PcbStackMode.Simple ? Diameter : Diameters[0];
        set => SetDiameterTop(value);
    }

    public Coordinate DiameterMiddle {
        get => DiameterStackMode == PcbStackMode.Simple ? Diameter : Diameters[1];
        set => SetDiameterMiddle(value);
    }

    public Coordinate DiameterBottom {
        get => DiameterStackMode == PcbStackMode.Simple ? Diameter : Diameters[Diameters.Count - 1];
        set => SetDiameterBottom(value);
    }

    public PcbVia() : base() {
        Diameter = Coordinate.FromMils(50);
        HoleSize = Coordinate.FromMils(28);
        FromLayer = 1;
        ToLayer = 32;
        ThermalReliefAirGapWidth = Coordinate.FromMils(10);
        ThermalReliefConductors = 4;
        ThermalReliefConductorsWidth = Coordinate.FromMils(10);
        SolderMaskExpansionManual = false;
        SolderMaskExpansion = Coordinate.FromMils(4);
    }

    internal List<Layer> GetParts() {
        var result = new List<Layer>();
        if (ToLayer.Name == "BottomLayer" && !IsTentingBottom) {
            result.Add(LayerMetadata.Get("BottomSolder").Id);
        }
        if (FromLayer.Name == "TopLayer" && !IsTentingTop) {
            result.Add(LayerMetadata.Get("TopSolder").Id);
        }

        result.Add(Layer);
        result.Add(LayerMetadata.Get("ViaHoleLayer").Id);

        if (FromLayer.Name != "TopLayer" || ToLayer.Name != "BottomLayer") {
            result.Add(FromLayer);
            result.Add(ToLayer);
        }

        return result;
    }

    internal CoordinateRectangular CalculatePartRect(LayerMetadata metadata, bool useAbsolutePosition) {
        var solderMaskExpansion = SolderMaskExpansionManual
            ? SolderMaskExpansion
            : MilsToCoordinate(8);
        var solderMaskExpansionTop = IsTentingTop
            ? MilsToCoordinate(0)
            : solderMaskExpansion;
        var solderMaskExpansionBottom = IsTentingBottom
            ? MilsToCoordinate(0)
            : solderMaskExpansion;

        Coordinate diameter;

        if (metadata.Name == "TopSolder") {
            diameter = DiameterTop + solderMaskExpansionTop;
        } else if (metadata.Name == "BottomSolder") {
            diameter = DiameterBottom + solderMaskExpansionBottom;
        } else if (metadata.Name == "MultiLayer") {
            diameter = Diameter;
        } else {
            diameter = HoleSize;
        }

        Coordinate x = -diameter / 2;
        Coordinate y = -diameter / 2;
        if (useAbsolutePosition) {
            x += Location.X;
            y += Location.Y;
        }

        return new CoordinateRectangular(x, y, diameter, diameter);
    }

    public override CoordinateRectangular CalculateBounds() {
        var result = CoordinateRectangular.Empty;
        foreach (var p in GetParts()) {
            result = CoordinateRectangular.Union(result, CalculatePartRect(p.Metadata, true));
        }
        return result;
    }

    private void SetDiameterAll(Coordinate value) {
        for (var i = 0; i < Diameters.Count; ++i) {
            Diameters[i] = value;
        }
    }

    private void SetDiameterTop(Coordinate value) {
        if (DiameterStackMode == PcbStackMode.Simple) {
            SetDiameterAll(value);
        } else {
            Diameters[0] = value;
        }
    }

    private void SetDiameterMiddle(Coordinate value) {
        if (DiameterStackMode == PcbStackMode.Simple) {
            SetDiameterAll(value);
        } else {
            for (var i = 1; i < Diameters.Count - 1; ++i) {
                Diameters[i] = value;
            }
        }
    }

    private void SetDiameterBottom(Coordinate value) {
        if (DiameterStackMode == PcbStackMode.Simple) {
            SetDiameterAll(value);
        } else {
            Diameters[Diameters.Count - 1] = value;
        }
    }
}
