using System.Globalization;
using System.Text.RegularExpressions;

using Avalonia.Media;

using CircuitCraftLab.AltiumFormats.UnitsSystem;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbLibHeader {
    public string Filename { get; internal set; } = string.Empty!;

    public static string Kind => "Protel_Advanced_PCB_Library";

    public string Version { get; private set; }

    public string Date { get; set; }

    public string Time { get; set; }

    public int V9MasterStackStyle { get; internal set; }

    public string V9MasterStackId { get; internal set; } = string.Empty!;

    public string V9MasterStackName { get; internal set; } = string.Empty!;

    public bool V9MasterStackShowTopDielectric { get; internal set; }

    public bool V9MasterStackShowBottomDielectric { get; internal set; }

    public bool V9MasterStackIsFlex { get; internal set; }

    public List<(string Id, string Name, int LayerId, bool UsedByPrims,
        int DielType, double DielConst, Coordinate DielHeight, string DielMaterial,
        Coordinate COverLayEXPansiOn, Coordinate CopThick, int ComponentPlacement)>
        V9StackLayer { get; internal set; }

    public List<(int LayerId, bool UsedByPrims, string Id, string Name,
        int DielType, double DielConst, Coordinate DielHeight, string DielMaterial,
        Coordinate COverLayEXPansiOn, Coordinate CopThick,
        int ComponentPlacement, Coordinate PullBackDistance, bool MechEnabled)>
        V9CacheLayer { get; internal set; }

    public int LayerMasterStackV8Style { get; internal set; }

    public string LayerMasterStackV8Id { get; internal set; } = string.Empty!;

    public string LayerMasterStackV8Name { get; internal set; } = string.Empty!;

    public bool LayerMasterStackV8ShowTopDielectric { get; internal set; }

    public bool LayerMasterStackV8ShowBottomDielectric { get; internal set; }

    public bool LayerMasterStackV8IsFlex { get; internal set; }

    public List<(string Id, string Name, int LayerId, bool UsedByPrims,
        int DielType, double DielConst, Coordinate DielHeight, string DielMaterial,
        Coordinate COverLayEXPansiOn, Coordinate CopThick,
        int ComponentPlacement, bool MechEnabled)>
        LayerV8 { get; internal set; }

    public int TopType { get; set; }

    public double TopConst { get; set; }

    public Coordinate TopHeight { get; set; }

    public string TopMaterial { get; set; }

    public int BottomType { get; set; }

    public double BottomConst { get; set; }

    public Coordinate BottomHeight { get; set; }

    public string BottomMaterial { get; set; }

    public int LayerStackStyle { get; set; }

    public bool ShowTopDielectric { get; set; }

    public bool ShowBottomDielectric { get; set; }

    public List<(string Name, int Prev, int Next, bool MechEnabled,
        Coordinate CopThick, int DielType, double DielConst,
        Coordinate DielHeight, string DielMaterial)>
        Layer { get; internal set; }

    public List<(string Name, int Prev, int Next, bool MechEnabled,
        Coordinate CopThick, int DielType, double DielConst, Coordinate DielHeight,
        string DielMaterial, int LayerId)>
        LayerV7 { get; internal set; }

    public Coordinate BigVisibleGridSize { get; set; }

    public Coordinate VisibleGridSize { get; set; }

    public Coordinate SnapGridSize { get; set; }

    public Coordinate SnapGridSizeX { get; set; }

    public Coordinate SnapGridSizeY { get; set; }

    public string LibGridsNGuide { get; set; } = string.Empty!;

    public Coordinate ElectricalGridRange { get; set; }

    public bool ElectricalGridEnabled { get; set; }

    public bool DotGrid { get; set; }

    public bool DotGridLarge { get; set; }

    public Unit DisplayUnit { get; set; }

    public string ToggleLayers { get; set; }

    public bool ShowDefaultSets { get; set; }

    public List<(string Name, string Layers, string ActiveLayer,
        bool IsCurrent, bool IsLocked, bool FlipBoard)>
        Layersets { get; internal set; }

    public int CfgAllConfigurationKind { get; private set; }

    public string CfgAllConfigurationDesc { get; private set; } = string.Empty!;

    public Color CfgAllComponentBodyRefPointColor { get; private set; }

    public string Cfg2DPrimDrawMode { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityTopLayer { get; set; } = string.Empty!;

    public List<string> Cfg2DLayerOpacityMidLayer { get; set; }

    public List<string> Cfg2DLayerOpacityMechanicalLayer { get; set; }

    public string Cfg2DLayerOpacityBottomLayer { get; private set; } = string.Empty!;

    public string Cfg2DLayerOpacityTopOverlay { get; private set; } = string.Empty!;

    public string Cfg2DLayerOpacityBottomOverlay { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityTopPaste { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityBottomPaste { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityTopSolder { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityBottomSolder { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane1 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane2 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane3 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane4 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane5 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane6 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane7 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane8 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane9 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane10 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane11 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane12 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane13 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane14 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane15 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityInternalPlane16 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityDrillGuide { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityKeepoutLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityDrillDrawing { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityMultiLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityCOnNectLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityBackGroundLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityDrcErrorLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityHighlightLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityGridColor1 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityGridColor10 { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityPadHoleLayer { get; set; } = string.Empty!;

    public string Cfg2DLayerOpacityViaHoleLayer { get; set; } = string.Empty!;

    public string Cfg2DToggleLayers { get; set; } = string.Empty!;

    public string Cfg2DToggleLayersSet { get; set; } = string.Empty!;

    public List<KeyValuePair<int, double>> Cfg2DWorkspaceColAlpha { get; set; }

    public string Cfg2DMechLayerInSingleLayerMode { get; set; } = string.Empty!;

    public string Cfg2DMechLayerInSingleLayerModeSet { get; set; } = string.Empty!;

    public string Cfg2DLayersInSingleLayerModeSet { get; private set; } = string.Empty!;

    public string Cfg2DMechLayerLinkedToSheet { get; set; } = string.Empty!;

    public string Cfg2DMechLayerLinkedToSheetSet { get; set; } = string.Empty!;

    public string Cfg2DCurrentLayer { get; set; } = string.Empty!;

    public bool Cfg2DDisplaySpecialStrings { get; set; }

    public bool Cfg2DShowTestPoints { get; set; }

    public bool Cfg2DShowOriginMarker { get; set; }

    public int Cfg2DEyeDist { get; set; }

    public bool Cfg2DShowStatusInfo { get; set; }

    public bool Cfg2DShowPadNets { get; set; }

    public bool Cfg2DShowPadNumberS { get; set; }

    public bool Cfg2DShowViaNets { get; set; }

    public bool Cfg2DUSetRansparentLayers { get; set; }

    public int Cfg2DPlaneDrawMode { get; set; }

    public int Cfg2DDisplayNetNamesOnTracks { get; set; }

    public int Cfg2DFromToSDisplayMode { get; set; }

    public int Cfg2DPadTypeSDisplayMode { get; set; }

    public int Cfg2DSingleLayerModeState { get; set; }

    public Color Cfg2DOriginMarkerColor { get; set; }

    public bool Cfg2DShowComponentRefPoint { get; set; }

    public Color Cfg2DComponentRefPointColor { get; set; }

    public bool Cfg2DPosItiveTopSolderMask { get; set; }

    public bool Cfg2DPosItiveBottomSolderMask { get; set; }

    public double Cfg2DTopPosItivesolderMaskAlpha { get; set; }

    public double Cfg2DBottomPosItivesolderMaskAlpha { get; set; }

    public bool Cfg2DAllConnectionsInSingleLayerMode { get; set; }

    public bool Cfg2DMultiColoredConnections { get; set; }

    public bool Cfg2DShowSpecialStringsHandles { get; private set; }

    public bool Cfg2DMechanicalCoverLayerUpdated { get; private set; }

    public string BoardInsightViewConfigurationName { get; set; } = string.Empty!;

    public double VisibleGridMultFactor { get; set; }

    public double BigVisibleGridMultFactor { get; set; }

    public string Current2D3DViewState { get; set; }

    public CoordinateRectangular ViewPort { get; set; }

    public string Property2DConfigType { get; set; }

    public string Property2DConfiguration { get; set; }

    public string Property2DConfigFullFilename { get; set; }

    public string Property3DConfigType { get; set; }

    public string Property3DConfiguration { get; set; }

    public string Property3DConfigFullFilename { get; set; }

    public CoordinatePoint3D LookAt { get; set; }

    public double EyeRotationX { get; set; }

    public double EyeRotationY { get; set; }

    public double EyeRotationZ { get; set; }

    public double ZoomMult { get; set; }

    public CoordinatePoint ViewSize { get; set; }

    public Coordinate EgRange { get; set; }

    public double EgMult { get; set; }

    public bool EgEnabled { get; set; }

    public bool EgSnapToBoardOutline { get; set; }

    public bool EgSnapToArcCenters { get; set; }

    public bool EgUseAllLayers { get; set; }

    public bool OgSnapEnabled { get; set; }

    public bool MgSnapEnabled { get; set; }

    public bool PointGuideEnabled { get; set; }

    public bool GridSnapEnabled { get; set; }

    public bool NearObjectsEnabled { get; set; }

    public bool FarObjectsEnabled { get; set; }

    public string NearObjectSet { get; set; } = string.Empty!;

    public string FarObjectSet { get; set; } = string.Empty!;

    public Coordinate NearDistance { get; set; }

    public double BoardVersion { get; set; }

    public string VaultGuid { get; set; } = string.Empty!;

    public string FolderGuid { get; set; } = string.Empty!;

    public string LifeCycleDefinitionGuid { get; set; } = string.Empty!;

    public string RevisionNamingSchemeGuid { get; set; } = string.Empty!;

    public Color CfgAllComponentBodySnapPointColor { get; private set; }

    public bool CfgAllShowComponentSnapMarkers { get; private set; }

    public bool CfgAllShowComponentSnapReference { get; private set; }

    public bool CfgAllShowComponentSnapCustom { get; private set; }

    public bool Cfg2DShowViaSpan { get; private set; }

    public PcbLibHeader() {
        Version = "3.00";
        Date = DateTime.Now.ToShortDateString();
        Time = DateTime.Now.ToLongTimeString();

        TopType = 3;
        TopConst = 3.5;
        TopHeight = Coordinate.FromMils(0.4);
        TopMaterial = "Solder Resist";

        BottomType = 3;
        BottomConst = 3.5;
        BottomHeight = Coordinate.FromMils(0.4);
        BottomMaterial = "Solder Resist";

        Layer = new() {
            ("Top Layer", 0, 32, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 1", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 2", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 3", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 4", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 5", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 6", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 7", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 8", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 9", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 10", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 11", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 12", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 13", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 14", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 15", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 16", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 17", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 18", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 19", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 20", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 21", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 22", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 23", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 24", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 25", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 26", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 27", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 28", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 29", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mid-Layer 30", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Bottom Layer", 1, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Top Overlay", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Bottom Overlay", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Top Paste", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Bottom Paste", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Top Solder", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Bottom Solder", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 1", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 2", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 3", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 4", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 5", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 6", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 7", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 8", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 9", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 10", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 11", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 12", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 13", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 14", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 15", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Internal Plane 16", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Drill Guide", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Keep-Out Layer", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 1", 0, 0, true, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 2", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 3", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 4", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 5", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 6", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 7", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 8", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 9", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 10", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 11", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 12", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 13", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 14", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 15", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Mechanical 16", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Drill Drawing", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Multi-Layer", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Connections", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Background", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("DRC Error Markers", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Selections", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Visible Grid 1", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Visible Grid 2", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Pad Holes", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4"),
            ("Via Holes", 0, 0, false, Coordinate.FromMils(1.4), 0, 4.8, Coordinate.FromMils(12.6), "FR-4")
        };

        LayerV7 = new() {
            ("Mechanical 17", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908305)),
            ("Mechanical 18", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908306)),
            ("Mechanical 19", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908307)),
            ("Mechanical 20", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908308)),
            ("Mechanical 21", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908309)),
            ("Mechanical 22", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908310)),
            ("Mechanical 23", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908311)),
            ("Mechanical 24", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908312)),
            ("Mechanical 25", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908313)),
            ("Mechanical 26", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908314)),
            ("Mechanical 27", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908315)),
            ("Mechanical 28", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908316)),
            ("Mechanical 29", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908317)),
            ("Mechanical 30", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908318)),
            ("Mechanical 31", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908319)),
            ("Mechanical 32", 16973824, 16973824, false, Coordinate.FromMils(0), 0, 4.8, Coordinate.FromMils(0), "FR-4", Coordinate.FromInt32(16908320))
        };

        LayerV8 = new();
        V9StackLayer = new();
        V9CacheLayer = new();

        BigVisibleGridSize = 0;
        VisibleGridSize = 0;

        SnapGridSize = 50000;
        SnapGridSizeX = 50000;
        SnapGridSizeY = 50000;

        ElectricalGridRange = Coordinate.FromMils(8);
        ElectricalGridEnabled = true;
        DisplayUnit = Unit.Mil;
        ToggleLayers = "1111111111111111111111111111111111111111111111111111111111111111111111111111111111";
        ShowDefaultSets = true;

        Layersets = new() {
            ("&All Layers", "TopLayer,BottomLayer,TopOverlay,BottomOverlay,TopPaste,BottomPaste,TopSolder,BottomSolder,DrillGuide,KeepOutLayer,Mechanical1,DrillDrawing,MultiLayer", "TOP", false, true, false),
            ("&Signal Layers", "TopLayer,BottomLayer,MultiLayer", "TOP", false, true, false),
            ("&Plane Layers", "", "UNKNOWN", false, true, false),
            ("&NonSignal Layers", "TopOverlay,BottomOverlay,TopPaste,BottomPaste,TopSolder,BottomSolder,DrillGuide,KeepOutLayer,DrillDrawing,MultiLayer", "TOPOVERLAY", false, true, false),
            ("&Mechanical Layers", "Mechanical1", "MECHANICAL1", false, true, false)
        };

        Cfg2DToggleLayers = "1111111111111111111111111111111111111111111111111111111111111111111111111111111111";
        Cfg2DToggleLayersSet = "Signal.All~0_Signal.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16777217=1,16777218=1,16777219=1,16777220=1,16777221=1,16777222=1,16777223=1,16777224=1,16777225=1,16777226=1,16777227=1,16777228=1,16777229=1,16777230=1,16777231=1,16777232=1,16777233=1,16777234=1,16777235=1,16777236=1,16777237=1,16777238=1,16777239=1,16777240=1,16777241=1,16777242=1,16777243=1,16777244=1,16777245=1,16777246=1,16777247=1,16842751=1_Mechanical.All~0_Mechanical.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16908289=1,16908290=1,16908291=1,16908292=1,16908293=1,16908294=1,16908295=1,16908296=1,16908297=1,16908298=1,16908299=1,16908300=1,16908301=1,16908302=1,16908303=1,16908304=1_Internal.All~0_Internal.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16842753=1,16842754=1,16842755=1,16842756=1,16842757=1,16842758=1,16842759=1,16842760=1,16842761=1,16842762=1,16842763=1,16842764=1,16842765=1,16842766=1,16842767=1,16842768=1_Standard.All~0_Standard.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16973850=1,16973830=1,16973831=1,16973832=1,16973833=1,16973834=1,16973835=1,16973836=1,16973837=1,16973838=1,16973839=1,16973840=1,16973841=1,16973842=1,16973843=1,16973844=1,16973845=1,16973846=1,16973847=1";
        Cfg2DWorkspaceColAlpha = new();
        Cfg2DMechLayerInSingleLayerModeSet = "SerializeLayerHash.Version~2,ClassName~TLayerToBoolean,25165826~0";
        Cfg2DMechLayerLinkedToSheetSet = "SerializeLayerHash.Version~2,ClassName~TLayerToBoolean,25165826~0";
        Cfg2DCurrentLayer = "TOP";
        Cfg2DShowOriginMarker = true;
        Cfg2DEyeDist = 2000;
        Cfg2DShowStatusInfo = true;
        Cfg2DShowPadNets = true;
        Cfg2DShowPadNumberS = true;
        Cfg2DShowViaNets = true;
        Cfg2DPlaneDrawMode = 2;
        Cfg2DDisplayNetNamesOnTracks = 1;
        Cfg2DSingleLayerModeState = 3;
        Cfg2DOriginMarkerColor = Color.FromUInt32(16777215);
        Cfg2DComponentRefPointColor = Color.FromUInt32(16777215);
        Cfg2DTopPosItivesolderMaskAlpha = 0.5;
        Cfg2DBottomPosItivesolderMaskAlpha = 0.5;
        Cfg2DLayerOpacityMidLayer = new List<string>();
        Cfg2DLayerOpacityMechanicalLayer = new List<string>();

        VisibleGridMultFactor = 1;
        BigVisibleGridMultFactor = 5;
        Current2D3DViewState = "2D";

        ViewPort = new CoordinateRectangular(
            Coordinate.FromInt32(394607096),
            Coordinate.FromInt32(109393894),
            Coordinate.FromInt32(415241036 - 394607096),
            Coordinate.FromInt32(115878846 - 109393894));

        Property2DConfigType = ".config_2dsimple";
        Property2DConfiguration = @"`RECORD=Board`CFGALL.CONFIGURATIONKIND=1`CFGALL.CONFIGURATIONDESC=Altium%20Standard%202D`CFG2D.PRIMDRAWMODE=00000000000000000000000`CFG2D.LAYEROPACITY=1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?1.0?`CFG2D.LAYEROPACITY.SET=SerializeLayerHash.Version~2,ClassName~TLayerToSingle,25165829~1065353216`CFG2D.TOGGLELAYERS=1111111111111111111111111111111111111111111111111111111111111111111111111111111111`CFG2D.TOGGLELAYERS.SET=Signal.All~0_Signal.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16777217=1,16777218=1,16777219=1,16777220=1,16777221=1,16777222=1,16777223=1,16777224=1,16777225=1,16777226=1,16777227=1,16777228=1,16777229=1,16777230=1,16777231=1,16777232=1,16777233=1,16777234=1,16777235=1,16777236=1,16777237=1,16777238=1,16777239=1,16777240=1,16777241=1,16777242=1,16777243=1,16777244=1,16777245=1,16777246=1,16777247=1,16842751=1_Mechanical.All~0_Mechanical.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16908289=1,16908290=1,16908291=1,16908292=1,16908293=1,16908294=1,16908295=1,16908296=1,16908297=1,16908298=1,16908299=1,16908300=1,16908301=1,16908302=1,16908303=1,16908304=1_Internal.All~0_Internal.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16842753=1,16842754=1,16842755=1,16842756=1,16842757=1,16842758=1,16842759=1,16842760=1,16842761=1,16842762=1,16842763=1,16842764=1,16842765=1,16842766=1,16842767=1,16842768=1_Standard.All~0_Standard.Include~SerializeLayerHash.Version=2,ClassName=TLayerHash,16973850=1,16973830=1,16973831=1,16973832=1,16973833=1,16973834=1,16973835=1,16973836=1,16973837=1,16973838=1,16973839=1,16973840=1,16973841=1,16973842=1,16973843=1,16973844=1,16973845=1,16973846=1,16973847=1`CFG2D.WORKSPACECOLALPHA0=1.0`CFG2D.WORKSPACECOLALPHA1=1.0`CFG2D.WORKSPACECOLALPHA2=1.0`CFG2D.WORKSPACECOLALPHA3=1.0`CFG2D.WORKSPACECOLALPHA4=1.0`CFG2D.WORKSPACECOLALPHA5=1.0`CFG2D.WORKSPACECOLALPHA6=1.0`CFG2D.MECHLAYERINSINGLELAYERMODE=0000000000000000`CFG2D.MECHLAYERINSINGLELAYERMODE.SET=SerializeLayerHash.Version~2,ClassName~TLayerToBoolean,25165826~0`CFG2D.MECHLAYERLINKEDTOSHEET=0000000000000000`CFG2D.MECHLAYERLINKEDTOSHEET.SET=SerializeLayerHash.Version~2,ClassName~TLayerToBoolean,25165826~0`CFG2D.CURRENTLAYER=TOP`CFG2D.DISPLAYSPECIALSTRINGS=FALSE`CFG2D.SHOWTESTPOINTS=FALSE`CFG2D.SHOWORIGINMARKER=TRUE`CFG2D.EYEDIST=2000`CFG2D.SHOWSTATUSINFO=TRUE`CFG2D.SHOWPADNETS=TRUE`CFG2D.SHOWPADNUMBERS=TRUE`CFG2D.SHOWVIANETS=TRUE`CFG2D.USETRANSPARENTLAYERS=FALSE`CFG2D.PLANEDRAWMODE=2`CFG2D.DISPLAYNETNAMESONTRACKS=1`CFG2D.FROMTOSDISPLAYMODE=0`CFG2D.PADTYPESDISPLAYMODE=0`CFG2D.SINGLELAYERMODESTATE=3`CFG2D.ORIGINMARKERCOLOR=16777215`CFG2D.SHOWCOMPONENTREFPOINT=FALSE`CFG2D.COMPONENTREFPOINTCOLOR=16777215`CFG2D.POSITIVETOPSOLDERMASK=FALSE`CFG2D.POSITIVEBOTTOMSOLDERMASK=FALSE`CFG2D.TOPPOSITIVESOLDERMASKALPHA=0.500000`CFG2D.BOTTOMPOSITIVESOLDERMASKALPHA=0.500000";
        Property2DConfigFullFilename = "(Not Saved)";
        Property3DConfigType = ".config_3d";
        Property3DConfiguration = "";
        Property3DConfigFullFilename = "(Not Saved)";

        LookAt = new CoordinatePoint3D(
            Coordinate.FromInt32(0),
            Coordinate.FromInt32(0),
            Coordinate.FromInt32(0));

        EyeRotationX = 0;
        EyeRotationY = 0;
        EyeRotationZ = 0;
        ZoomMult = 1E-06;

        ViewSize = new CoordinatePoint(
            Coordinate.FromInt32(377),
            Coordinate.FromInt32(343));

        EgRange = Coordinate.FromMils(8);
        EgMult = 0;
        EgEnabled = true;
        GridSnapEnabled = true;
        NearObjectsEnabled = true;
        FarObjectsEnabled = true;
        NearObjectSet = "011111100011100000000000001";
        FarObjectSet = "001100000000000000000000000";
        NearDistance = Coordinate.FromMils(1000);
    }

    public void ImportFromParameters(ParameterCollection p) {
        if (p == null) {
            throw new ArgumentNullException(nameof(p));
        }

        Filename = p["FILENAME"].AsStringOrDefault()!;
        var kind = p["KIND"].AsStringOrDefault();
        if (kind != Kind) {
            throw new ArgumentOutOfRangeException($"{nameof(p)}[\"KIND\"]");
        }

        Version = p["VERSION"].AsStringOrDefault()!;
        Date = p["DATE"].AsStringOrDefault()!;
        Time = p["TIME"].AsStringOrDefault()!;
        V9MasterStackStyle = p["V9_MASTERSTACK_STYLE"].AsIntOrDefault();
        V9MasterStackId = p["V9_MASTERSTACK_ID"].AsStringOrDefault()!;
        V9MasterStackName = p["V9_MASTERSTACK_NAME"].AsStringOrDefault()!;
        V9MasterStackShowTopDielectric = p["V9_MASTERSTACK_SHOWTOPDIELECTRIC"].AsBool();
        V9MasterStackShowBottomDielectric = p["V9_MASTERSTACK_SHOWBOTTOMDIELECTRIC"].AsBool();
        V9MasterStackIsFlex = p["V9_MASTERSTACK_ISFLEX"].AsBool();

        var v9StackLayerMin = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "V9_STACK_LAYER{0}_ID", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .DefaultIfEmpty("0")
            .Min(v => int.Parse(v, CultureInfo.InvariantCulture));
        var v9StackLayerCount = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "V9_STACK_LAYER{0}_ID", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .Count();

        V9StackLayer = new();

        for (var i = v9StackLayerMin; i < v9StackLayerCount; i++) {
            var dielHeight = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_STACK_LAYER{0}_DIELHEIGHT", i)) && p.Contains(Format("V9_STACK_LAYER{0}_DIELHEIGHT_FRAC", i))) {
                dielHeight = DotsPerPixelFractionToCoordinate(p[Format("V9_STACK_LAYER{0}_DIELHEIGHT", i)].AsIntOrDefault(), p[Format("V9_STACK_LAYER{0}_DIELHEIGHT_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_STACK_LAYER{0}_DIELHEIGHT", i))) {
                dielHeight = Unit.StringToCoordUnit(p[Format("V9_STACK_LAYER{0}_DIELHEIGHT", i)].AsStringOrDefault()!);
            }

            var coverlayExpansion = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION", i)) && p.Contains(Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION_FRAC", i))) {
                coverlayExpansion = DotsPerPixelFractionToCoordinate(p[Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION", i)].AsIntOrDefault(), p[Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION", i))) {
                coverlayExpansion = Unit.StringToCoordUnit(p[Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION", i)].AsStringOrDefault()!);
            }

            var copperThickness = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_STACK_LAYER{0}_COPTHICK", i)) && p.Contains(Format("V9_STACK_LAYER{0}_COPTHICK_FRAC", i))) {
                copperThickness = DotsPerPixelFractionToCoordinate(p[Format("V9_STACK_LAYER{0}_COPTHICK", i)].AsIntOrDefault(), p[Format("V9_STACK_LAYER{0}_COPTHICK_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_STACK_LAYER{0}_COPTHICK", i))) {
                copperThickness = Unit.StringToCoordUnit(p[Format("V9_STACK_LAYER{0}_COPTHICK", i)].AsStringOrDefault()!);
            }

            V9StackLayer.Add(
                (
                    p[Format("V9_STACK_LAYER{0}_ID", i)].AsStringOrDefault()!,
                    p[Format("V9_STACK_LAYER{0}_NAME", i)].AsStringOrDefault()!,
                    p[Format("V9_STACK_LAYER{0}_LAYERID", i)].AsIntOrDefault()!,
                    p[Format("V9_STACK_LAYER{0}_USEDBYPRIMS", i)].AsBool()!,
                    p[Format("V9_STACK_LAYER{0}_DIELTYPE", i)].AsIntOrDefault()!,
                    p[Format("V9_STACK_LAYER{0}_DIELCONST", i)].AsDoubleOrDefault()!,
                    dielHeight,
                    p[Format("V9_STACK_LAYER{0}_DIELMATERIAL", i)].AsStringOrDefault()!,
                    coverlayExpansion,
                    copperThickness,
                    p[Format("V9_STACK_LAYER{0}_COMPONENTPLACEMENT", i)].AsIntOrDefault()!
                )
            );
        }

        var v9CacheLayerMin = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "V9_CACHE_LAYER{0}_LAYERID", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .DefaultIfEmpty("0")
            .Min(v => int.Parse(v, CultureInfo.InvariantCulture));
        var v9CacheLayerCount = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "V9_CACHE_LAYER{0}_LAYERID", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .Count();

        V9CacheLayer = new();

        for (var i = v9CacheLayerMin; i < v9CacheLayerCount; i++) {
            var dielHeight = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_CACHE_LAYER{0}_DIELHEIGHT", i)) && p.Contains(Format("V9_CACHE_LAYER{0}_DIELHEIGHT_FRAC", i))) {
                dielHeight = DotsPerPixelFractionToCoordinate(p[Format("V9_CACHE_LAYER{0}_DIELHEIGHT", i)].AsIntOrDefault(), p[Format("V9_CACHE_LAYER{0}_DIELHEIGHT_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_CACHE_LAYER{0}_DIELHEIGHT", i))) {
                dielHeight = Unit.StringToCoordUnit(p[Format("V9_CACHE_LAYER{0}_DIELHEIGHT", i)].AsStringOrDefault()!);
            }

            var coverlayExpansion = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION", i)) && p.Contains(Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION_FRAC", i))) {
                coverlayExpansion = DotsPerPixelFractionToCoordinate(p[Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION", i)].AsIntOrDefault(), p[Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION", i))) {
                coverlayExpansion = Unit.StringToCoordUnit(p[Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION", i)].AsStringOrDefault()!);
            }

            var copperThickness = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_CACHE_LAYER{0}_COPTHICK", i)) && p.Contains(Format("V9_CACHE_LAYER{0}_COPTHICK_FRAC", i))) {
                copperThickness = DotsPerPixelFractionToCoordinate(p[Format("V9_CACHE_LAYER{0}_COPTHICK", i)].AsIntOrDefault(), p[Format("V9_CACHE_LAYER{0}_COPTHICK_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_CACHE_LAYER{0}_COPTHICK", i))) {
                copperThickness = Unit.StringToCoordUnit(p[Format("V9_CACHE_LAYER{0}_COPTHICK", i)].AsStringOrDefault()!);
            }

            var pullback = Coordinate.FromMils(0);
            if (p.Contains(Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE", i)) && p.Contains(Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE_FRAC", i))) {
                pullback = DotsPerPixelFractionToCoordinate(p[Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE", i)].AsIntOrDefault(), p[Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE", i))) {
                pullback = Unit.StringToCoordUnit(p[Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE", i)].AsStringOrDefault()!);
            }

            V9CacheLayer.Add(
                (
                    p[Format("V9_CACHE_LAYER{0}_LAYERID", i)].AsIntOrDefault()!,
                    p[Format("V9_CACHE_LAYER{0}_USEDBYPRIMS", i)].AsBool(),
                    p[Format("V9_CACHE_LAYER{0}_ID", i)].AsStringOrDefault()!,
                    p[Format("V9_CACHE_LAYER{0}_NAME", i)].AsStringOrDefault()!,
                    p[Format("V9_CACHE_LAYER{0}_DIELTYPE", i)].AsIntOrDefault()!,
                    p[Format("V9_CACHE_LAYER{0}_DIELCONST", i)].AsDoubleOrDefault()!,
                    dielHeight,
                    p[Format("V9_CACHE_LAYER{0}_DIELMATERIAL", i)].AsStringOrDefault()!,
                    coverlayExpansion,
                    copperThickness,
                    p[Format("V9_CACHE_LAYER{0}_COMPONENTPLACEMENT", i)].AsIntOrDefault()!,
                    pullback,
                    p[Format("V9_CACHE_LAYER{0}_MECHENABLED", i)].AsBool()
                )
            );
        }

        LayerMasterStackV8Style = p["LAYERMASTERSTACK_V8STYLE"].AsIntOrDefault();
        LayerMasterStackV8Id = p["LAYERMASTERSTACK_V8ID"].AsStringOrDefault()!;
        LayerMasterStackV8Name = p["LAYERMASTERSTACK_V8NAME"].AsStringOrDefault()!;
        LayerMasterStackV8ShowTopDielectric = p["LAYERMASTERSTACK_V8SHOWTOPDIELECTRIC"].AsBool();
        LayerMasterStackV8ShowBottomDielectric = p["LAYERMASTERSTACK_V8SHOWBOTTOMDIELECTRIC"].AsBool();
        LayerMasterStackV8IsFlex = p["LAYERMASTERSTACK_V8ISFLEX"].AsBool();

        var v8LayerMin = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "LAYER_V8_{0}ID", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .DefaultIfEmpty("0")
            .Min(v => int.Parse(v, CultureInfo.InvariantCulture));
        var v8LayerCount = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "LAYER_V8_{0}ID", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .Count();

        LayerV8 = new();

        for (var i = v8LayerMin; i < v8LayerCount; i++) {
            var dielHeight = Coordinate.FromMils(0);
            if (p.Contains(Format("LAYER_V8_{0}DIELHEIGHT", i)) && p.Contains(Format("LAYER_V8_{0}DIELHEIGHT_FRAC", i))) {
                dielHeight = DotsPerPixelFractionToCoordinate(p[Format("LAYER_V8_{0}DIELHEIGHT", i)].AsIntOrDefault(), p[Format("LAYER_V8_{0}DIELHEIGHT_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("LAYER_V8_{0}DIELHEIGHT", i))) {
                dielHeight = Unit.StringToCoordUnit(p[Format("LAYER_V8_{0}DIELHEIGHT", i)].AsStringOrDefault()!);
            }

            var coverlayExpansion = Coordinate.FromMils(0);
            if (p.Contains(Format("LAYER_V8_{0}COVERLAY_EXPANSION", i)) && p.Contains(Format("LAYER_V8_{0}COVERLAY_EXPANSION_FRAC", i))) {
                coverlayExpansion = DotsPerPixelFractionToCoordinate(p[Format("LAYER_V8_{0}COVERLAY_EXPANSION", i)].AsIntOrDefault(), p[Format("LAYER_V8_{0}COVERLAY_EXPANSION_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("LAYER_V8_{0}COVERLAY_EXPANSION", i))) {
                coverlayExpansion = Unit.StringToCoordUnit(p[Format("LAYER_V8_{0}COVERLAY_EXPANSION", i)].AsStringOrDefault()!);
            }

            var copperThickness = Coordinate.FromMils(0);
            if (p.Contains(Format("LAYER_V8_{0}COPTHICK", i)) && p.Contains(Format("LAYER_V8_{0}COPTHICK_FRAC", i))) {
                copperThickness = DotsPerPixelFractionToCoordinate(p[Format("LAYER_V8_{0}COPTHICK", i)].AsIntOrDefault(), p[Format("LAYER_V8_{0}COPTHICK_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("LAYER_V8_{0}COPTHICK", i))) {
                copperThickness = Unit.StringToCoordUnit(p[Format("LAYER_V8_{0}COPTHICK", i)].AsStringOrDefault()!);
            }

            LayerV8.Add(
                (
                    p[Format("LAYER_V8_{0}ID", i)].AsStringOrDefault()!,
                    p[Format("LAYER_V8_{0}NAME", i)].AsStringOrDefault()!,
                    p[Format("LAYER_V8_{0}LAYERID", i)].AsIntOrDefault()!,
                    p[Format("LAYER_V8_{0}USEDBYPRIMS", i)].AsBool(),
                    p[Format("LAYER_V8_{0}DIELTYPE", i)].AsIntOrDefault()!,
                    p[Format("LAYER_V8_{0}DIELCONST", i)].AsDoubleOrDefault()!,
                    dielHeight,
                    p[Format("LAYER_V8_{0}DIELMATERIAL", i)].AsStringOrDefault()!,
                    coverlayExpansion,
                    copperThickness,
                    p[Format("LAYER_V8_{0}COMPONENTPLACEMENT", i)].AsIntOrDefault()!,
                    p[Format("LAYER_V8_{0}MECHENABLED", i)].AsBool()
                )
            );
        }

        TopType = p["TOPTYPE"].AsIntOrDefault();
        TopConst = p["TOPCONST"].AsDoubleOrDefault();
        TopHeight = p["TOPHEIGHT"].AsCoordinate();
        TopMaterial = p["TOPMATERIAL"].AsStringOrDefault()!;
        BottomType = p["BOTTOMTYPE"].AsIntOrDefault();
        BottomConst = p["BOTTOMCONST"].AsDoubleOrDefault();
        BottomHeight = p["BOTTOMHEIGHT"].AsCoordinate();
        BottomMaterial = p["BOTTOMMATERIAL"].AsStringOrDefault()!;
        LayerStackStyle = p["LAYERSTACKSTYLE"].AsIntOrDefault();
        ShowTopDielectric = p["SHOWTOPDIELECTRIC"].AsBool();
        ShowBottomDielectric = p["SHOWBOTTOMDIELECTRIC"].AsBool();

        Layer = Enumerable.Range(
                p
                    .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "LAYER{0}NAME", @"(\d+)")).Groups[1].Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .DefaultIfEmpty("0")
                    .Min(v => int.Parse(v, CultureInfo.InvariantCulture)),
                p
                    .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "LAYER{0}NAME", @"(\d+)")).Groups[1].Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Count())
            .Select(i => (
                p[$"LAYER{i}NAME"].AsStringOrDefault()!,
                p[$"LAYER{i}PREV"].AsIntOrDefault(),
                p[$"LAYER{i}NEXT"].AsIntOrDefault(),
                p[$"LAYER{i}MECHENABLED"].AsBool(),
                p[$"LAYER{i}COPTHICK"].AsCoordinate(),
                p[$"LAYER{i}DIELTYPE"].AsIntOrDefault()!,
                p[$"LAYER{i}DIELCONST"].AsDoubleOrDefault()!,
                p[$"LAYER{i}DIELHEIGHT"].AsCoordinate(),
                p[$"LAYER{i}DIELMATERIAL"].AsStringOrDefault()!))
            .ToList();

        var v7LayerMin = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "LAYERV7_{0}NAME", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .DefaultIfEmpty("0")
            .Min(v => int.Parse(v, CultureInfo.InvariantCulture));
        var v7LayerCount = p
            .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "LAYERV7_{0}NAME", @"(\d+)")).Groups[1].Value)
            .Where(v => !string.IsNullOrEmpty(v))
            .Count();

        LayerV7 = new();

        for (var i = v7LayerMin; i < v7LayerCount; i++) {
            var dielHeight = Coordinate.FromMils(0);
            if (p.Contains(Format("LAYERV7_{0}DIELHEIGHT", i)) && p.Contains(Format("LAYERV7_{0}DIELHEIGHT_FRAC", i))) {
                dielHeight = DotsPerPixelFractionToCoordinate(p[Format("LAYERV7_{0}DIELHEIGHT", i)].AsIntOrDefault(), p[Format("LAYERV7_{0}DIELHEIGHT_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("LAYERV7_{0}DIELHEIGHT", i))) {
                dielHeight = Unit.StringToCoordUnit(p[Format("LAYERV7_{0}DIELHEIGHT", i)].AsStringOrDefault()!);
            }

            var copperThickness = Coordinate.FromMils(0);
            if (p.Contains(Format("LAYERV7_{0}COPTHICK", i)) && p.Contains(Format("LAYERV7_{0}COPTHICK_FRAC", i))) {
                copperThickness = DotsPerPixelFractionToCoordinate(p[Format("LAYERV7_{0}COPTHICK", i)].AsIntOrDefault(), p[Format("LAYERV7_{0}COPTHICK_FRAC", i)].AsIntOrDefault());
            } else if (p.Contains(Format("LAYERV7_{0}COPTHICK", i))) {
                copperThickness = Unit.StringToCoordUnit(p[Format("LAYERV7_{0}COPTHICK", i)].AsStringOrDefault()!);
            }

            LayerV7.Add(
                (
                    p[Format("LAYERV7_{0}NAME", i)].AsStringOrDefault()!,
                    p[Format("LAYERV7_{0}PREV", i)].AsIntOrDefault()!,
                    p[Format("LAYERV7_{0}NEXT", i)].AsIntOrDefault()!,
                    p[Format("LAYERV7_{0}MECHENABLED", i)].AsBool(),
                    copperThickness,
                    p[Format("LAYERV7_{0}DIELTYPE", i)].AsIntOrDefault()!,
                    p[Format("LAYERV7_{0}DIELCONST", i)].AsDoubleOrDefault()!,
                    dielHeight,
                    p[Format("LAYERV7_{0}DIELMATERIAL", i)].AsStringOrDefault()!,
                    p[Format("LAYERV7_{0}LAYERID", i)].AsIntOrDefault()!
                )
            );
        }

        BigVisibleGridSize = Coordinate.FromInt32((int) p["BIGVISIBLEGRIDSIZE"].AsDoubleOrDefault());
        VisibleGridSize = Coordinate.FromInt32((int) p["VISIBLEGRIDSIZE"].AsDoubleOrDefault());
        SnapGridSize = (int) p["SNAPGRIDSIZE"].AsDoubleOrDefault();
        SnapGridSizeX = (int) p["SNAPGRIDSIZEX"].AsDoubleOrDefault();
        SnapGridSizeY = (int) p["SNAPGRIDSIZEY"].AsDoubleOrDefault();
        LibGridsNGuide = p["LIBGRIDSNGUIDE"].AsStringOrDefault()!;
        ElectricalGridRange = p["ELECTRICALGRIDRANGE"].AsCoordinate();
        ElectricalGridEnabled = p["ELECTRICALGRIDENABLED"].AsBool();
        DotGrid = p["DOTGRID"].AsBool();
        DotGridLarge = p["DOTGRIDLARGE"].AsBool();
        DisplayUnit = p["DISPLAYUNIT"].AsIntOrDefault() == 0 ? Unit.Millimeter : Unit.Mil;
        ToggleLayers = p["TOGGLELAYERS"].AsStringOrDefault()!;
        ShowDefaultSets = p["SHOWDEFAULTSETS"].AsBool();

        Layersets = Enumerable.Range(1, p["LAYERSETSCOUNT"].AsInt())
            .Select(i => (
                p[Format("LAYERSET{0}NAME", i)].AsStringOrDefault()!,
                p[Format("LAYERSET{0}LAYERS", i)].AsStringOrDefault()!,
                p[Format("LAYERSET{0}ACTIVELAYER.7", i)].AsStringOrDefault()!,
                p[Format("LAYERSET{0}ISCURRENT", i)].AsBool(),
                p[Format("LAYERSET{0}ISLOCKED", i)].AsBool(),
                p[Format("LAYERSET{0}FLIPBOARD", i)].AsBool()))
            .ToList();

        CfgAllConfigurationKind = p["CFGALL.CONFIGURATIONKIND"].AsIntOrDefault();
        CfgAllConfigurationDesc = p["CFGALL.CONFIGURATIONDESC"].AsStringOrDefault()!;
        CfgAllComponentBodyRefPointColor = p["CFGALL.COMPONENTBODYREFPOINTCOLOR"].AsColorOrDefault();
        CfgAllComponentBodySnapPointColor = p["CFGALL.COMPONENTBODYSNAPPOINTCOLOR"].AsColorOrDefault();
        CfgAllShowComponentSnapMarkers = p["CFGALL.SHOWCOMPONENTSNAPMARKERS"].AsBool();
        CfgAllShowComponentSnapReference = p["CFGALL.SHOWCOMPONENTSNAPREFERENCE"].AsBool();
        CfgAllShowComponentSnapCustom = p["CFGALL.SHOWCOMPONENTSNAPCUSTOM"].AsBool();
        Cfg2DPrimDrawMode = p["CFG2D.PRIMDRAWMODE"].AsStringOrDefault()!;
        Cfg2DLayerOpacityTopLayer = p["CFG2D.LAYEROPACITY.TOPLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityMidLayer = Enumerable.Range(
                p
                    .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "CFG2D.LAYEROPACITY.MIDLAYER{0}", @"(\d+)")).Groups[1].Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .DefaultIfEmpty("0")
                    .Min(v => int.Parse(v, CultureInfo.InvariantCulture)),
                p
                    .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "CFG2D.LAYEROPACITY.MIDLAYER{0}", @"(\d+)")).Groups[1].Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Count())
            .Select(i =>
                p[Format("CFG2D.LAYEROPACITY.MIDLAYER{0}", i)].AsStringOrDefault()!)
            .ToList();
        Cfg2DLayerOpacityBottomLayer = p["CFG2D.LAYEROPACITY.BOTTOMLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityTopOverlay = p["CFG2D.LAYEROPACITY.TOPOVERLAY"].AsStringOrDefault()!;
        Cfg2DLayerOpacityBottomOverlay = p["CFG2D.LAYEROPACITY.BOTTOMOVERLAY"].AsStringOrDefault()!;
        Cfg2DLayerOpacityTopPaste = p["CFG2D.LAYEROPACITY.TOPPASTE"].AsStringOrDefault()!;
        Cfg2DLayerOpacityBottomPaste = p["CFG2D.LAYEROPACITY.BOTTOMPASTE"].AsStringOrDefault()!;
        Cfg2DLayerOpacityTopSolder = p["CFG2D.LAYEROPACITY.TOPSOLDER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityBottomSolder = p["CFG2D.LAYEROPACITY.BOTTOMSOLDER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane1 = p["CFG2D.LAYEROPACITY.INTERNALPLANE1"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane2 = p["CFG2D.LAYEROPACITY.INTERNALPLANE2"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane3 = p["CFG2D.LAYEROPACITY.INTERNALPLANE3"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane4 = p["CFG2D.LAYEROPACITY.INTERNALPLANE4"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane5 = p["CFG2D.LAYEROPACITY.INTERNALPLANE5"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane6 = p["CFG2D.LAYEROPACITY.INTERNALPLANE6"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane7 = p["CFG2D.LAYEROPACITY.INTERNALPLANE7"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane8 = p["CFG2D.LAYEROPACITY.INTERNALPLANE8"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane9 = p["CFG2D.LAYEROPACITY.INTERNALPLANE9"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane10 = p["CFG2D.LAYEROPACITY.INTERNALPLANE10"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane11 = p["CFG2D.LAYEROPACITY.INTERNALPLANE11"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane12 = p["CFG2D.LAYEROPACITY.INTERNALPLANE12"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane13 = p["CFG2D.LAYEROPACITY.INTERNALPLANE13"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane14 = p["CFG2D.LAYEROPACITY.INTERNALPLANE14"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane15 = p["CFG2D.LAYEROPACITY.INTERNALPLANE15"].AsStringOrDefault()!;
        Cfg2DLayerOpacityInternalPlane16 = p["CFG2D.LAYEROPACITY.INTERNALPLANE16"].AsStringOrDefault()!;
        Cfg2DLayerOpacityDrillGuide = p["CFG2D.LAYEROPACITY.DRILLGUIDE"].AsStringOrDefault()!;
        Cfg2DLayerOpacityKeepoutLayer = p["CFG2D.LAYEROPACITY.KEEPOUTLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityMechanicalLayer = Enumerable.Range(
                p
                    .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "CFG2D.LAYEROPACITY.MECHANICAL{0}", @"(\d+)")).Groups[1].Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .DefaultIfEmpty("0")
                    .Min(v => int.Parse(v, CultureInfo.InvariantCulture)),
                p
                    .Select(kv => Regex.Match(kv.key, string.Format(CultureInfo.InvariantCulture, "CFG2D.LAYEROPACITY.MECHANICAL{0}", @"(\d+)")).Groups[1].Value)
                    .Where(v => !string.IsNullOrEmpty(v))
                    .Count())
            .Select(i =>
                p[Format("CFG2D.LAYEROPACITY.MECHANICAL{0}", i)].AsStringOrDefault()!)
            .ToList();
        Cfg2DLayerOpacityDrillDrawing = p["CFG2D.LAYEROPACITY.DRILLDRAWING"].AsStringOrDefault()!;
        Cfg2DLayerOpacityMultiLayer = p["CFG2D.LAYEROPACITY.MULTILAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityCOnNectLayer = p["CFG2D.LAYEROPACITY.CONNECTLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityBackGroundLayer = p["CFG2D.LAYEROPACITY.BACKGROUNDLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityDrcErrorLayer = p["CFG2D.LAYEROPACITY.DRCERRORLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityHighlightLayer = p["CFG2D.LAYEROPACITY.HIGHLIGHTLAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityGridColor1 = p["CFG2D.LAYEROPACITY.GRIDCOLOR1"].AsStringOrDefault()!;
        Cfg2DLayerOpacityGridColor10 = p["CFG2D.LAYEROPACITY.GRIDCOLOR10"].AsStringOrDefault()!;
        Cfg2DLayerOpacityPadHoleLayer = p["CFG2D.LAYEROPACITY.PADHOLELAYER"].AsStringOrDefault()!;
        Cfg2DLayerOpacityViaHoleLayer = p["CFG2D.LAYEROPACITY.VIAHOLELAYER"].AsStringOrDefault()!;
        Cfg2DToggleLayers = p["CFG2D.TOGGLELAYERS"].AsStringOrDefault()!;
        Cfg2DToggleLayersSet = p["CFG2D.TOGGLELAYERS.SET"].AsStringOrDefault()!;

        Cfg2DWorkspaceColAlpha = p.Where(n => Regex.IsMatch(n.key, "CFG2D.WORKSPACECOLALPHA\\d+", RegexOptions.CultureInvariant))
            .Select(n => new KeyValuePair<int, double>(int.Parse(Regex.Match(n.key, "CFG2D.WORKSPACECOLALPHA(\\d+)", RegexOptions.CultureInvariant).Groups[1].Value), p[n.key].AsDoubleOrDefault()))
            .ToList();
        Cfg2DMechLayerInSingleLayerMode = p["CFG2D.MECHLAYERINSINGLELAYERMODE"].AsStringOrDefault()!;
        Cfg2DMechLayerInSingleLayerModeSet = p["CFG2D.MECHLAYERINSINGLELAYERMODE.SET"].AsStringOrDefault()!;
        Cfg2DLayersInSingleLayerModeSet = p["CFG2D.LAYERSINSINGLELAYERMODE.SET"].AsStringOrDefault()!;
        Cfg2DMechLayerLinkedToSheet = p["CFG2D.MECHLAYERLINKEDTOSHEET"].AsStringOrDefault()!;
        Cfg2DMechLayerLinkedToSheetSet = p["CFG2D.MECHLAYERLINKEDTOSHEET.SET"].AsStringOrDefault()!;
        Cfg2DCurrentLayer = p["CFG2D.CURRENTLAYER"].AsStringOrDefault()!;
        Cfg2DDisplaySpecialStrings = p["CFG2D.DISPLAYSPECIALSTRINGS"].AsBool();
        Cfg2DShowTestPoints = p["CFG2D.SHOWTESTPOINTS"].AsBool();
        Cfg2DShowOriginMarker = p["CFG2D.SHOWORIGINMARKER"].AsBool();
        Cfg2DEyeDist = p["CFG2D.EYEDIST"].AsIntOrDefault();
        Cfg2DShowStatusInfo = p["CFG2D.SHOWSTATUSINFO"].AsBool();
        Cfg2DShowPadNets = p["CFG2D.SHOWPADNETS"].AsBool();
        Cfg2DShowPadNumberS = p["CFG2D.SHOWPADNUMBERS"].AsBool();
        Cfg2DShowViaNets = p["CFG2D.SHOWVIANETS"].AsBool();
        Cfg2DShowViaSpan = p["CFG2D.SHOWVIASPAN"].AsBool();
        Cfg2DUSetRansparentLayers = p["CFG2D.USETRANSPARENTLAYERS"].AsBool();
        Cfg2DPlaneDrawMode = p["CFG2D.PLANEDRAWMODE"].AsIntOrDefault();
        Cfg2DDisplayNetNamesOnTracks = p["CFG2D.DISPLAYNETNAMESONTRACKS"].AsIntOrDefault();
        Cfg2DFromToSDisplayMode = p["CFG2D.FROMTOSDISPLAYMODE"].AsIntOrDefault();
        Cfg2DPadTypeSDisplayMode = p["CFG2D.PADTYPESDISPLAYMODE"].AsIntOrDefault();
        Cfg2DSingleLayerModeState = p["CFG2D.SINGLELAYERMODESTATE"].AsIntOrDefault();
        Cfg2DOriginMarkerColor = p["CFG2D.ORIGINMARKERCOLOR"].AsColorOrDefault();
        Cfg2DShowComponentRefPoint = p["CFG2D.SHOWCOMPONENTREFPOINT"].AsBool();
        Cfg2DComponentRefPointColor = p["CFG2D.COMPONENTREFPOINTCOLOR"].AsColorOrDefault();
        Cfg2DPosItiveTopSolderMask = p["CFG2D.POSITIVETOPSOLDERMASK"].AsBool();
        Cfg2DPosItiveBottomSolderMask = p["CFG2D.POSITIVEBOTTOMSOLDERMASK"].AsBool();
        Cfg2DTopPosItivesolderMaskAlpha = p["CFG2D.TOPPOSITIVESOLDERMASKALPHA"].AsDoubleOrDefault();
        Cfg2DBottomPosItivesolderMaskAlpha = p["CFG2D.BOTTOMPOSITIVESOLDERMASKALPHA"].AsDoubleOrDefault();
        Cfg2DAllConnectionsInSingleLayerMode = p["CFG2D.ALLCONNECTIONSINSINGLELAYERMODE"].AsBool();
        Cfg2DMultiColoredConnections = p["CFG2D.MULTICOLOREDCONNECTIONS"].AsBool();
        Cfg2DShowSpecialStringsHandles = p["CFG2D.SHOWSPECIALSTRINGSHANDLES"].AsBool();
        Cfg2DMechanicalCoverLayerUpdated = p["CFG2D.MECHCOVERLAYERUPDATED"].AsBool();
        BoardInsightViewConfigurationName = p["BOARDINSIGHTVIEWCONFIGURATIONNAME"].AsStringOrDefault()!;
        VisibleGridMultFactor = p["VISIBLEGRIDMULTFACTOR"].AsDoubleOrDefault();
        BigVisibleGridMultFactor = p["BIGVISIBLEGRIDMULTFACTOR"].AsDoubleOrDefault();
        Current2D3DViewState = p["CURRENT2D3DVIEWSTATE"].AsStringOrDefault()!;
        ViewPort = new CoordinateRectangular(
            Coordinate.FromInt32(p["VP.LX"].AsIntOrDefault()),
            Coordinate.FromInt32(p["VP.LY"].AsIntOrDefault()),
            Coordinate.FromInt32(p["VP.HX"].AsIntOrDefault() - p["VP.LX"].AsIntOrDefault()),
            Coordinate.FromInt32(p["VP.HY"].AsIntOrDefault() - p["VP.LY"].AsIntOrDefault()));
        Property2DConfigType = p["2DCONFIGTYPE"].AsStringOrDefault()!;
        Property2DConfiguration = p["2DCONFIGURATION"].AsStringOrDefault()!;
        Property2DConfigFullFilename = p["2DCONFIGFULLFILENAME"].AsStringOrDefault()!;
        Property3DConfigType = p["3DCONFIGTYPE"].AsStringOrDefault()!;
        Property3DConfiguration = p["3DCONFIGURATION"].AsStringOrDefault()!;
        Property3DConfigFullFilename = p["3DCONFIGFULLFILENAME"].AsStringOrDefault()!;
        LookAt = new CoordinatePoint3D(
            Coordinate.FromInt32((int) p["LOOKAT.X"].AsDoubleOrDefault()),
            Coordinate.FromInt32((int) p["LOOKAT.Y"].AsDoubleOrDefault()),
            Coordinate.FromInt32((int) p["LOOKAT.Z"].AsDoubleOrDefault()));
        EyeRotationX = p["EYEROTATION.X"].AsDoubleOrDefault();
        EyeRotationY = p["EYEROTATION.Y"].AsDoubleOrDefault();
        EyeRotationZ = p["EYEROTATION.Z"].AsDoubleOrDefault();
        ZoomMult = p["ZOOMMULT"].AsDoubleOrDefault();
        ViewSize = new CoordinatePoint(
            Coordinate.FromInt32((int) p["VIEWSIZE.X"].AsDoubleOrDefault()),
            Coordinate.FromInt32((int) p["VIEWSIZE.Y"].AsDoubleOrDefault()));
        EgRange = p["EGRANGE"].AsCoordinate();
        EgMult = p["EGMULT"].AsDoubleOrDefault();
        EgEnabled = p["EGENABLED"].AsBool();
        EgSnapToBoardOutline = p["EGSNAPTOBOARDOUTLINE"].AsBool();
        EgUseAllLayers = p["EGUSEALLLAYERS"].AsBool();
        EgSnapToArcCenters = p["EGSNAPTOARCCENTERS"].AsBool();
        OgSnapEnabled = p["OGSNAPENABLED"].AsBool();
        MgSnapEnabled = p["MGSNAPENABLED"].AsBool();
        PointGuideEnabled = p["POINTGUIDEENABLED"].AsBool();
        GridSnapEnabled = p["GRIDSNAPENABLED"].AsBool();
        NearObjectsEnabled = p["NEAROBJECTSENABLED"].AsBool();
        FarObjectsEnabled = p["FAROBJECTSENABLED"].AsBool();
        NearObjectSet = p["NEAROBJECTSET"].AsStringOrDefault()!;
        FarObjectSet = p["FAROBJECTSET"].AsStringOrDefault()!;
        NearDistance = p["NEARDISTANCE"].AsCoordinate();
        BoardVersion = p["BOARDVERSION"].AsDoubleOrDefault()!;
        VaultGuid = p["VAULTGUID"].AsStringOrDefault()!;
        FolderGuid = p["FOLDERGUID"].AsStringOrDefault()!;
        LifeCycleDefinitionGuid = p["LIFECYCLEDEFINITIONGUID"].AsStringOrDefault()!;
        RevisionNamingSchemeGuid = p["REVISIONNAMINGSCHEMEGUID"].AsStringOrDefault()!;
    }

    private static void AddParamRecord(ParameterCollection p) {
        if (p.Contains("RECORD")) {
            p.AddKey("RECORD", true);
        } else {
            p.Add("RECORD", "Board");
        }
    }

    public void ExportToParameters(ParameterCollection p) {
        if (p == null) throw new ArgumentNullException(nameof(p));

        p.UseLongBooleans = true;

        p.Add("FILENAME", Filename);
        p.Add("KIND", Kind);
        p.Add("VERSION", Version);
        p.Add("DATE", Date);
        p.Add("TIME", Time);
        if (V9StackLayer.Count > 0) {
            p.Add("V9_MASTERSTACK_STYLE", V9MasterStackStyle, false);
            p.Add("V9_MASTERSTACK_ID", V9MasterStackId);
            p.Add("V9_MASTERSTACK_NAME", V9MasterStackName);
            p.Add("V9_MASTERSTACK_SHOWTOPDIELECTRIC", V9MasterStackShowTopDielectric, false);
            p.Add("V9_MASTERSTACK_SHOWBOTTOMDIELECTRIC", V9MasterStackShowBottomDielectric, false);
            p.Add("V9_MASTERSTACK_ISFLEX", V9MasterStackIsFlex, false);
            for (var i = 0; i < V9StackLayer.Count; i++) {
                p.Add(Format("V9_STACK_LAYER{0}_ID", i), V9StackLayer[i].Id);
                p.Add(Format("V9_STACK_LAYER{0}_NAME", i), V9StackLayer[i].Name);
                p.Add(Format("V9_STACK_LAYER{0}_LAYERID", i), V9StackLayer[i].LayerId);
                p.Add(Format("V9_STACK_LAYER{0}_USEDBYPRIMS", i), V9StackLayer[i].UsedByPrims, false);
                p.Add(Format("V9_STACK_LAYER{0}_DIELTYPE", i), V9StackLayer[i].DielType, V9StackLayer[i].DielConst == 0);
                p.Add(Format("V9_STACK_LAYER{0}_DIELCONST", i), V9StackLayer[i].DielConst);
                p.Add(Format("V9_STACK_LAYER{0}_DIELHEIGHT", i), V9StackLayer[i].DielHeight);
                p.Add(Format("V9_STACK_LAYER{0}_DIELMATERIAL", i), V9StackLayer[i].DielMaterial);
                p.Add(Format("V9_STACK_LAYER{0}_COVERLAY_EXPANSION", i), V9StackLayer[i].COverLayEXPansiOn, !(V9StackLayer[i].Name == "Top Solder" || V9StackLayer[i].Name == "Bottom Solder" || V9StackLayer[i].DielMaterial == "Solder Resist"));
                p.Add(Format("V9_STACK_LAYER{0}_COPTHICK", i), V9StackLayer[i].CopThick);
                p.Add(Format("V9_STACK_LAYER{0}_COMPONENTPLACEMENT", i), V9StackLayer[i].ComponentPlacement);
            }
            for (var i = 0; i < V9CacheLayer.Count; i++) {
                p.Add(Format("V9_CACHE_LAYER{0}_ID", i), V9CacheLayer[i].Id);
                p.Add(Format("V9_CACHE_LAYER{0}_NAME", i), V9CacheLayer[i].Name);
                p.Add(Format("V9_CACHE_LAYER{0}_LAYERID", i), V9CacheLayer[i].LayerId);
                p.Add(Format("V9_CACHE_LAYER{0}_USEDBYPRIMS", i), V9CacheLayer[i].UsedByPrims, false);
                p.Add(Format("V9_CACHE_LAYER{0}_DIELTYPE", i), V9CacheLayer[i].DielType, V9CacheLayer[i].DielConst == 0);
                p.Add(Format("V9_CACHE_LAYER{0}_DIELCONST", i), V9CacheLayer[i].DielConst);
                p.Add(Format("V9_CACHE_LAYER{0}_DIELHEIGHT", i), V9CacheLayer[i].DielHeight);
                p.Add(Format("V9_CACHE_LAYER{0}_DIELMATERIAL", i), V9CacheLayer[i].DielMaterial);
                p.Add(Format("V9_CACHE_LAYER{0}_COVERLAY_EXPANSION", i), V9CacheLayer[i].COverLayEXPansiOn, !(V9CacheLayer[i].Name == "Top Solder" || V9CacheLayer[i].Name == "Bottom Solder" || V9CacheLayer[i].DielMaterial == "Solder Resist"));
                p.Add(Format("V9_CACHE_LAYER{0}_COPTHICK", i), V9CacheLayer[i].CopThick);
                p.Add(Format("V9_CACHE_LAYER{0}_COMPONENTPLACEMENT", i), V9CacheLayer[i].ComponentPlacement, V9CacheLayer[i].CopThick.ToMils() == 0);
                p.Add(Format("V9_CACHE_LAYER{0}_PULLBACKDISTANCE", i), V9CacheLayer[i].PullBackDistance);
                p.Add(Format("V9_CACHE_LAYER{0}_MECHENABLED", i), V9CacheLayer[i].MechEnabled, i < 70);
            }
        }
        if (LayerV8.Count > 0) {
            p.Add("LAYERMASTERSTACK_V8STYLE", LayerMasterStackV8Style, false);
            p.Add("LAYERMASTERSTACK_V8ID", LayerMasterStackV8Id);
            p.Add("LAYERMASTERSTACK_V8NAME", LayerMasterStackV8Name);
            p.Add("LAYERMASTERSTACK_V8SHOWTOPDIELECTRIC", LayerMasterStackV8ShowTopDielectric, false);
            p.Add("LAYERMASTERSTACK_V8SHOWBOTTOMDIELECTRIC", LayerMasterStackV8ShowBottomDielectric, false);
            p.Add("LAYERMASTERSTACK_V8ISFLEX", LayerMasterStackV8IsFlex, false);
            for (var i = 0; i < LayerV8.Count; i++) {
                p.Add(Format("LAYER_V8_{0}ID", i), LayerV8[i].Id);
                p.Add(Format("LAYER_V8_{0}NAME", i), LayerV8[i].Name);
                p.Add(Format("LAYER_V8_{0}LAYERID", i), LayerV8[i].LayerId);
                p.Add(Format("LAYER_V8_{0}USEDBYPRIMS", i), LayerV8[i].UsedByPrims, false);
                p.Add(Format("LAYER_V8_{0}DIELTYPE", i), LayerV8[i].DielType, LayerV8[i].DielConst == 0);
                p.Add(Format("LAYER_V8_{0}DIELCONST", i), LayerV8[i].DielConst);
                p.Add(Format("LAYER_V8_{0}DIELHEIGHT", i), LayerV8[i].DielHeight);
                p.Add(Format("LAYER_V8_{0}DIELMATERIAL", i), LayerV8[i].DielMaterial);
                p.Add(Format("LAYER_V8_{0}COVERLAY_EXPANSION", i), LayerV8[i].COverLayEXPansiOn, !(LayerV8[i].Name == "Top Solder" || LayerV8[i].Name == "Bottom Solder" || LayerV8[i].DielMaterial == "Solder Resist"));
                p.Add(Format("LAYER_V8_{0}COPTHICK", i), LayerV8[i].CopThick);
                p.Add(Format("LAYER_V8_{0}COMPONENTPLACEMENT", i), LayerV8[i].ComponentPlacement);
                p.Add(Format("LAYER_V8_{0}MECHENABLED", i), LayerV8[i].MechEnabled, i < 15 || (i > 26 && i < 40));
            }
        }

        p.Add("TOPTYPE", TopType);
        p.Add("TOPCONST", TopConst, decimals: 3);
        p.Add("TOPHEIGHT", TopHeight);
        p.Add("TOPMATERIAL", TopMaterial);

        p.Add("BOTTOMTYPE", BottomType);
        p.Add("BOTTOMCONST", BottomConst, decimals: 3);
        p.Add("BOTTOMHEIGHT", BottomHeight);
        p.Add("BOTTOMMATERIAL", BottomMaterial);

        p.Add("LAYERSTACKSTYLE", LayerStackStyle, false);
        p.Add("SHOWTOPDIELECTRIC", ShowTopDielectric, false);
        p.Add("SHOWBOTTOMDIELECTRIC", ShowBottomDielectric, false);
        for (var i = 0; i < Layer.Count; i++) {
            if (i > 0 && i % 5 == 0) AddParamRecord(p);
            p.Add($"LAYER{i + 1}NAME", Layer[i].Name);
            p.Add($"LAYER{i + 1}PREV", Layer[i].Prev, false);
            p.Add($"LAYER{i + 1}NEXT", Layer[i].Next, false);
            p.Add($"LAYER{i + 1}MECHENABLED", Layer[i].MechEnabled, false);
            p.Add($"LAYER{i + 1}COPTHICK", Layer[i].CopThick, false);
            p.Add($"LAYER{i + 1}DIELTYPE", Layer[i].DielType, false);
            p.Add($"LAYER{i + 1}DIELCONST", Layer[i].DielConst, false, 3);
            p.Add($"LAYER{i + 1}DIELHEIGHT", Layer[i].DielHeight, false);
            p.Add($"LAYER{i + 1}DIELMATERIAL", Layer[i].DielMaterial, false);
        }
        for (var i = 0; i < LayerV7.Count; i++) {
            p.Add($"LAYERV7_{i}LAYERID", LayerV7[i].LayerId);
            p.Add($"LAYERV7_{i}NAME", LayerV7[i].Name);
            p.Add($"LAYERV7_{i}PREV", LayerV7[i].Prev, false);
            p.Add($"LAYERV7_{i}NEXT", LayerV7[i].Next, false);
            p.Add($"LAYERV7_{i}MECHENABLED", LayerV7[i].MechEnabled, false);
            p.Add($"LAYERV7_{i}COPTHICK", LayerV7[i].CopThick, false);
            p.Add($"LAYERV7_{i}DIELTYPE", LayerV7[i].DielType, false);
            p.Add($"LAYERV7_{i}DIELCONST", LayerV7[i].DielConst, false, 3);
            p.Add($"LAYERV7_{i}DIELHEIGHT", LayerV7[i].DielHeight, false);
            p.Add($"LAYERV7_{i}DIELMATERIAL", LayerV7[i].DielMaterial, false);
        }

        AddParamRecord(p);
        p.Add("BIGVISIBLEGRIDSIZE", (double) BigVisibleGridSize.ToInt32(), false);
        p.Add("VISIBLEGRIDSIZE", (double) VisibleGridSize.ToInt32(), false);
        p.Add("SNAPGRIDSIZE", (double) SnapGridSize.ToInt32());
        p.Add("SNAPGRIDSIZEX", (double) SnapGridSizeX.ToInt32());
        p.Add("SNAPGRIDSIZEY", (double) SnapGridSizeY.ToInt32());
        p.Add("LIBGRIDSNGUIDE", LibGridsNGuide);
        p.Add("ELECTRICALGRIDRANGE", ElectricalGridRange);
        p.Add("ELECTRICALGRIDENABLED", ElectricalGridEnabled);
        p.Add("DOTGRID", DotGrid);
        p.Add("DOTGRIDLARGE", DotGridLarge);
        p.Add("DISPLAYUNIT", DisplayUnit == Unit.Mil ? 1 : 0);
        p.Add("TOGGLELAYERS", ToggleLayers);
        p.Add("SHOWDEFAULTSETS", ShowDefaultSets);
        p.Add("LAYERSETSCOUNT", Layersets.Count);

        for (var i = 0; i < Layersets.Count; i++) {
            p.Add($"LAYERSET{i + 1}NAME", Layersets[i].Name);
            p.Add($"LAYERSET{i + 1}LAYERS", Layersets[i].Layers);
            p.Add($"LAYERSET{i + 1}ACTIVELAYER.7", Layersets[i].ActiveLayer);
            p.Add($"LAYERSET{i + 1}ISCURRENT", Layersets[i].IsCurrent, false);
            p.Add($"LAYERSET{i + 1}ISLOCKED", Layersets[i].IsLocked);
            p.Add($"LAYERSET{i + 1}FLIPBOARD", Layersets[i].FlipBoard, false);
        }

        AddParamRecord(p);
        p.Add("CFGALL.CONFIGURATIONKIND", CfgAllConfigurationKind);
        p.Add("CFGALL.CONFIGURATIONDESC", CfgAllConfigurationDesc);
        p.Add("CFGALL.COMPONENTBODYREFPOINTCOLOR", CfgAllComponentBodyRefPointColor.ToString());
        p.Add("CFGALL.COMPONENTBODYSNAPPOINTCOLOR", CfgAllComponentBodySnapPointColor.ToString());
        p.Add("CFGALL.SHOWCOMPONENTSNAPMARKERS", CfgAllShowComponentSnapMarkers, false);
        p.Add("CFGALL.SHOWCOMPONENTSNAPREFERENCE", CfgAllShowComponentSnapReference, false);
        p.Add("CFGALL.SHOWCOMPONENTSNAPCUSTOM", CfgAllShowComponentSnapCustom, false);
        p.Add("CFG2D.PRIMDRAWMODE", Cfg2DPrimDrawMode);
        p.Add("CFG2D.LAYEROPACITY.TOPLAYER", Cfg2DLayerOpacityTopLayer);

        for (var i = 0; i < Cfg2DLayerOpacityMidLayer.Count; i++) {
            p.Add($"CFG2D.LAYEROPACITY.MIDLAYER{i + 1}", Cfg2DLayerOpacityMidLayer[i]);
        }

        p.Add("CFG2D.LAYEROPACITY.BOTTOMLAYER", Cfg2DLayerOpacityBottomLayer);
        p.Add("CFG2D.LAYEROPACITY.TOPOVERLAY", Cfg2DLayerOpacityTopOverlay);
        p.Add("CFG2D.LAYEROPACITY.BOTTOMOVERLAY", Cfg2DLayerOpacityBottomOverlay);
        p.Add("CFG2D.LAYEROPACITY.TOPPASTE", Cfg2DLayerOpacityTopPaste);
        p.Add("CFG2D.LAYEROPACITY.BOTTOMPASTE", Cfg2DLayerOpacityBottomPaste);
        p.Add("CFG2D.LAYEROPACITY.TOPSOLDER", Cfg2DLayerOpacityTopSolder);
        p.Add("CFG2D.LAYEROPACITY.BOTTOMSOLDER", Cfg2DLayerOpacityBottomSolder);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE1", Cfg2DLayerOpacityInternalPlane1);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE2", Cfg2DLayerOpacityInternalPlane2);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE3", Cfg2DLayerOpacityInternalPlane3);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE4", Cfg2DLayerOpacityInternalPlane4);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE5", Cfg2DLayerOpacityInternalPlane5);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE6", Cfg2DLayerOpacityInternalPlane6);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE7", Cfg2DLayerOpacityInternalPlane7);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE8", Cfg2DLayerOpacityInternalPlane8);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE9", Cfg2DLayerOpacityInternalPlane9);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE10", Cfg2DLayerOpacityInternalPlane10);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE11", Cfg2DLayerOpacityInternalPlane11);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE12", Cfg2DLayerOpacityInternalPlane12);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE13", Cfg2DLayerOpacityInternalPlane13);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE14", Cfg2DLayerOpacityInternalPlane14);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE15", Cfg2DLayerOpacityInternalPlane15);
        p.Add("CFG2D.LAYEROPACITY.INTERNALPLANE16", Cfg2DLayerOpacityInternalPlane16);
        p.Add("CFG2D.LAYEROPACITY.DRILLGUIDE", Cfg2DLayerOpacityDrillGuide);
        p.Add("CFG2D.LAYEROPACITY.KEEPOUTLAYER", Cfg2DLayerOpacityKeepoutLayer);

        for (var i = 0; i < Cfg2DLayerOpacityMechanicalLayer.Count && i < 16; i++) {
            p.Add($"CFG2D.LAYEROPACITY.MECHANICAL{i + 1}", Cfg2DLayerOpacityMechanicalLayer[i]);
        }

        p.Add("CFG2D.LAYEROPACITY.DRILLDRAWING", Cfg2DLayerOpacityDrillDrawing);
        p.Add("CFG2D.LAYEROPACITY.MULTILAYER", Cfg2DLayerOpacityMultiLayer);
        p.Add("CFG2D.LAYEROPACITY.CONNECTLAYER", Cfg2DLayerOpacityCOnNectLayer);
        p.Add("CFG2D.LAYEROPACITY.BACKGROUNDLAYER", Cfg2DLayerOpacityBackGroundLayer);
        p.Add("CFG2D.LAYEROPACITY.DRCERRORLAYER", Cfg2DLayerOpacityDrcErrorLayer);
        p.Add("CFG2D.LAYEROPACITY.HIGHLIGHTLAYER", Cfg2DLayerOpacityHighlightLayer);
        p.Add("CFG2D.LAYEROPACITY.GRIDCOLOR1", Cfg2DLayerOpacityGridColor1);
        p.Add("CFG2D.LAYEROPACITY.GRIDCOLOR10", Cfg2DLayerOpacityGridColor10);
        p.Add("CFG2D.LAYEROPACITY.PADHOLELAYER", Cfg2DLayerOpacityPadHoleLayer);
        p.Add("CFG2D.LAYEROPACITY.VIAHOLELAYER", Cfg2DLayerOpacityViaHoleLayer);

        for (var i = 16; i < Cfg2DLayerOpacityMechanicalLayer.Count; i++) {
            p.Add($"CFG2D.LAYEROPACITY.MECHANICAL{i + 1}", Cfg2DLayerOpacityMechanicalLayer[i]);
        }

        p.Add("CFG2D.TOGGLELAYERS", Cfg2DToggleLayers);
        p.Add("CFG2D.TOGGLELAYERS.SET", Cfg2DToggleLayersSet);

        foreach (var a in Cfg2DWorkspaceColAlpha) {
            p.Add($"CFG2D.WORKSPACECOLALPHA{a.Key}", a.Value);
        }

        p.Add("CFG2D.MECHLAYERINSINGLELAYERMODE", Cfg2DMechLayerInSingleLayerMode);
        p.Add("CFG2D.MECHLAYERINSINGLELAYERMODE.SET", Cfg2DMechLayerInSingleLayerModeSet);
        p.Add("CFG2D.LAYERSINSINGLELAYERMODE.SET", Cfg2DLayersInSingleLayerModeSet);
        p.Add("CFG2D.MECHLAYERLINKEDTOSHEET", Cfg2DMechLayerLinkedToSheet);
        p.Add("CFG2D.MECHLAYERLINKEDTOSHEET.SET", Cfg2DMechLayerLinkedToSheetSet);
        p.Add("CFG2D.CURRENTLAYER", Cfg2DCurrentLayer);
        p.Add("CFG2D.DISPLAYSPECIALSTRINGS", Cfg2DDisplaySpecialStrings, false);
        p.Add("CFG2D.SHOWTESTPOINTS", Cfg2DShowTestPoints, false);
        p.Add("CFG2D.SHOWORIGINMARKER", Cfg2DShowOriginMarker, false);
        p.Add("CFG2D.EYEDIST", Cfg2DEyeDist);
        p.Add("CFG2D.SHOWSTATUSINFO", Cfg2DShowStatusInfo);
        p.Add("CFG2D.SHOWPADNETS", Cfg2DShowPadNets);
        p.Add("CFG2D.SHOWPADNUMBERS", Cfg2DShowPadNumberS);
        p.Add("CFG2D.SHOWVIANETS", Cfg2DShowViaNets);
        p.Add("CFG2D.SHOWVIASPAN", Cfg2DShowViaSpan);
        p.Add("CFG2D.USETRANSPARENTLAYERS", Cfg2DUSetRansparentLayers, false);
        p.Add("CFG2D.PLANEDRAWMODE", Cfg2DPlaneDrawMode);
        p.Add("CFG2D.DISPLAYNETNAMESONTRACKS", Cfg2DDisplayNetNamesOnTracks);
        p.Add("CFG2D.FROMTOSDISPLAYMODE", Cfg2DFromToSDisplayMode, false);
        p.Add("CFG2D.PADTYPESDISPLAYMODE", Cfg2DPadTypeSDisplayMode, false);
        p.Add("CFG2D.SINGLELAYERMODESTATE", Cfg2DSingleLayerModeState);
        p.Add("CFG2D.ORIGINMARKERCOLOR", Cfg2DOriginMarkerColor.ToString());
        p.Add("CFG2D.SHOWCOMPONENTREFPOINT", Cfg2DShowComponentRefPoint, false);
        p.Add("CFG2D.COMPONENTREFPOINTCOLOR", Cfg2DComponentRefPointColor.ToString());
        p.Add("CFG2D.POSITIVETOPSOLDERMASK", Cfg2DPosItiveTopSolderMask, false);
        p.Add("CFG2D.POSITIVEBOTTOMSOLDERMASK", Cfg2DPosItiveBottomSolderMask, false);
        p.Add("CFG2D.TOPPOSITIVESOLDERMASKALPHA", Cfg2DTopPosItivesolderMaskAlpha);
        p.Add("CFG2D.BOTTOMPOSITIVESOLDERMASKALPHA", Cfg2DBottomPosItivesolderMaskAlpha);
        p.Add("CFG2D.ALLCONNECTIONSINSINGLELAYERMODE", Cfg2DAllConnectionsInSingleLayerMode);
        p.Add("CFG2D.MULTICOLOREDCONNECTIONS", Cfg2DMultiColoredConnections, false);
        p.Add("CFG2D.SHOWSPECIALSTRINGSHANDLES", Cfg2DShowSpecialStringsHandles, false);
        p.Add("CFG2D.MECHCOVERLAYERUPDATED", Cfg2DMechanicalCoverLayerUpdated, false);
        p.Add("BOARDINSIGHTVIEWCONFIGURATIONNAME", BoardInsightViewConfigurationName, false);
        p.Add("VISIBLEGRIDMULTFACTOR", VisibleGridMultFactor, decimals: 3);
        p.Add("BIGVISIBLEGRIDMULTFACTOR", BigVisibleGridMultFactor, decimals: 3);

        AddParamRecord(p);
        p.Add("CURRENT2D3DVIEWSTATE", Current2D3DViewState);

        AddParamRecord(p);
        p.Add("VP.LX", ViewPort.Location1.X.ToInt32());
        p.Add("VP.HX", ViewPort.Location2.X.ToInt32());
        p.Add("VP.LY", ViewPort.Location1.Y.ToInt32());
        p.Add("VP.HY", ViewPort.Location2.Y.ToInt32());

        AddParamRecord(p);
        p.Add("2DCONFIGTYPE", Property2DConfigType);
        p.Add("2DCONFIGURATION", Property2DConfiguration);

        AddParamRecord(p);
        p.Add("2DCONFIGFULLFILENAME", Property2DConfigFullFilename);

        AddParamRecord(p);
        p.Add("3DCONFIGTYPE", Property3DConfigType);
        p.Add("3DCONFIGURATION", Property3DConfiguration);

        AddParamRecord(p);
        p.Add("3DCONFIGFULLFILENAME", Property3DConfigFullFilename);

        AddParamRecord(p);
        p.Add("LOOKAT.X", (double) LookAt.X.ToInt32(), false);
        p.Add("LOOKAT.Y", (double) LookAt.Y.ToInt32(), false);
        p.Add("LOOKAT.Z", (double) LookAt.Z.ToInt32(), false);
        p.Add("EYEROTATION.X", EyeRotationX, false);
        p.Add("EYEROTATION.Y", EyeRotationY, false);
        p.Add("EYEROTATION.Z", EyeRotationZ, false);
        p.Add("ZOOMMULT", ZoomMult, false);
        p.Add("VIEWSIZE.X", ViewSize.X.ToInt32());
        p.Add("VIEWSIZE.Y", ViewSize.Y.ToInt32());
        p.Add("EGRANGE", EgRange);
        p.Add("EGMULT", EgMult, false);
        p.Add("EGENABLED", EgEnabled);

        if (EgEnabled) {
            p.Add("EGSNAPTOBOARDOUTLINE", EgSnapToBoardOutline, false);
            p.Add("EGSNAPTOARCCENTERS", EgSnapToArcCenters);
            p.Add("EGUSEALLLAYERS", EgUseAllLayers, false);
        }

        p.Add("OGSNAPENABLED", OgSnapEnabled, false);
        p.Add("MGSNAPENABLED", MgSnapEnabled, false);
        p.Add("POINTGUIDEENABLED", PointGuideEnabled, false);
        p.Add("GRIDSNAPENABLED", GridSnapEnabled, false);
        p.Add("NEAROBJECTSENABLED", NearObjectsEnabled, false);
        p.Add("FAROBJECTSENABLED", FarObjectsEnabled, false);
        p.Add("NEAROBJECTSET", NearObjectSet);
        p.Add("FAROBJECTSET", FarObjectSet);
        p.Add("NEARDISTANCE", NearDistance);
        p.Add("BOARDVERSION", BoardVersion);
        p.Add("VAULTGUID", VaultGuid);
        p.Add("FOLDERGUID", FolderGuid);
        p.Add("LIFECYCLEDEFINITIONGUID", LifeCycleDefinitionGuid);
        p.Add("REVISIONNAMINGSCHEMEGUID", RevisionNamingSchemeGuid);
    }

    public ParameterCollection ExportToParameters() {
        var parameters = new ParameterCollection();
        ExportToParameters(parameters);
        return parameters;
    }

    private static string Format(string format, int i) =>
        string.Format(CultureInfo.InvariantCulture, format, i);
}
