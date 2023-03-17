using Avalonia.Media;

using CircuitCraftLab.AltiumFormats.UnitsSystem;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbComponentBody : PcbPrimitive {
    public override PcbPrimitiveObjectId ObjectId => PcbPrimitiveObjectId.ComponentBody;

    public List<CoordinatePoint> Outline { get; } = new();

    public string V7Layer { get; set; }

    public string Name { get; set; } = string.Empty!;

    public int Kind { get; set; }

    public int SubPolyIndex { get; set; }

    public int UnionIndex { get; set; }

    public Coordinate ArcResolution { get; set; }

    public bool IsShapeBased { get; set; }

    public Coordinate StandOffHeight { get; set; }

    public Coordinate OverallHeight { get; set; }

    public int BodyProjection { get; set; }

    public Color BodyColor3D { get; set; }

    public double BodyOpacity3D { get; set; }

    public string Identifier { get; set; } = string.Empty!;

    public string Texture { get; set; } = string.Empty!;

    public CoordinatePoint TextureCenter { get; set; }

    public CoordinatePoint TextureSize { get; set; }

    public double TextureRotation { get; set; }

    public string ModelId { get; set; } = string.Empty!;

    public int ModelChecksum { get; set; }

    public bool ModelEmbed { get; set; }

    public CoordinatePoint Model2DLocation { get; set; }

    public double Model2DRotation { get; set; }

    public double Model3DRotX { get; set; }

    public double Model3DRotY { get; set; }

    public double Model3DRotZ { get; set; }

    public Coordinate Model3DDz { get; set; }

    public int ModelSnapCount { get; set; }

    public int ModelType { get; set; }

    public string StepModel { get; set; } = string.Empty!;

    public PcbComponentBody() {
        V7Layer = "MECHANICAL1";
        SubPolyIndex = -1;
        BodyColor3D = Color.FromUInt32(14737632);
        BodyOpacity3D = 1.0;
        ModelEmbed = true;
        ModelType = 1;
    }

    public override CoordinateRectangular CalculateBounds() {
        return new CoordinateRectangular(
            new CoordinatePoint(Outline.Min(p => p.X), Outline.Min(p => p.Y)),
            new CoordinatePoint(Outline.Max(p => p.X), Outline.Max(p => p.Y)));
    }

    public void ImportFromParameters(ParameterCollection p) {
        if (p == null) {
            throw new ArgumentNullException(nameof(p));
        }

        V7Layer = p["V7_LAYER"].AsStringOrDefault()!;
        Name = p["NAME"].AsStringOrDefault()!;
        Kind = p["KIND"].AsIntOrDefault();
        SubPolyIndex = p["SUBPOLYINDEX"].AsIntOrDefault();
        UnionIndex = p["UNIONINDEX"].AsIntOrDefault();
        ArcResolution = p["ARCRESOLUTION"].AsCoordinate();
        IsShapeBased = p["ISSHAPEBASED"].AsBool();
        StandOffHeight = p["STANDOFFHEIGHT"].AsCoordinate();
        OverallHeight = p["OVERALLHEIGHT"].AsCoordinate();
        BodyProjection = p["BODYPROJECTION"].AsIntOrDefault();
        ArcResolution = p["ARCRESOLUTION"].AsCoordinate();
        BodyColor3D = p["BODYCOLOR3D"].AsColorOrDefault();
        BodyOpacity3D = p["BODYOPACITY3D"].AsDoubleOrDefault();
        Identifier = new string(p["IDENTIFIER"].AsIntList(',').Select(v => (char) v).ToArray());
        Texture = p["TEXTURE"].AsStringOrDefault()!;
        TextureCenter = new CoordinatePoint(p["TEXTURECENTERX"].AsCoordinate(), p["TEXTURECENTERY"].AsCoordinate());
        TextureSize = new CoordinatePoint(p["TEXTURESIZEX"].AsCoordinate(), p["TEXTURESIZEY"].AsCoordinate());
        TextureRotation = p["TEXTUREROTATION"].AsDouble();
        ModelId = p["MODELID"].AsStringOrDefault()!;
        ModelChecksum = p["MODEL.CHECKSUM"].AsIntOrDefault();
        ModelEmbed = p["MODEL.EMBED"].AsBool();
        Model2DLocation = new CoordinatePoint(p["MODEL.2D.X"].AsCoordinate(), p["MODEL.2D.Y"].AsCoordinate());
        Model2DRotation = p["MODEL.2D.ROTATION"].AsDoubleOrDefault();
        Model3DRotX = p["MODEL.3D.ROTX"].AsDoubleOrDefault();
        Model3DRotY = p["MODEL.3D.ROTY"].AsDoubleOrDefault();
        Model3DRotZ = p["MODEL.3D.ROTZ"].AsDoubleOrDefault();
        Model3DDz = p["MODEL.3D.DZ"].AsCoordinate();
        ModelSnapCount = p["MODEL.SNAPCOUNT"].AsIntOrDefault();
        ModelType = p["MODEL.MODELTYPE"].AsIntOrDefault();
    }

    public void ExportToParameters(ParameterCollection p) {
        if (p == null) {
            throw new ArgumentNullException(nameof(p));
        }

        p.UseLongBooleans = true;

        p.Add("V7_LAYER", V7Layer);
        p.Add("NAME", Name);
        p.Add("KIND", Kind);
        p.Add("SUBPOLYINDEX", SubPolyIndex);
        p.Add("UNIONINDEX", UnionIndex);
        p.Add("ARCRESOLUTION", ArcResolution);
        p.Add("ISSHAPEBASED", IsShapeBased);
        p.Add("STANDOFFHEIGHT", StandOffHeight);
        p.Add("OVERALLHEIGHT", OverallHeight);
        p.Add("BODYPROJECTION", BodyProjection);
        p.Add("ARCRESOLUTION", ArcResolution);
        p.Add("BODYCOLOR3D", BodyColor3D.ToString());
        p.Add("BODYOPACITY3D", BodyOpacity3D);
        p.Add("IDENTIFIER", string.Join(",", Identifier?.Select(c => (int) c) ?? Enumerable.Empty<int>()));
        p.Add("TEXTURE", Texture);
        p.Add("TEXTURECENTERX", TextureCenter.X);
        p.Add("TEXTURECENTERY", TextureCenter.Y);
        p.Add("TEXTURESIZEX", TextureSize.X);
        p.Add("TEXTURESIZEY", TextureSize.Y);
        p.Add("TEXTUREROTATION", TextureRotation);
        p.Add("MODELID", ModelId);
        p.Add("MODEL.CHECKSUM", ModelChecksum);
        p.Add("MODEL.EMBED", ModelEmbed);
        p.Add("MODEL.2D.X", Model2DLocation.X);
        p.Add("MODEL.2D.Y", Model2DLocation.Y);
        p.Add("MODEL.2D.ROTATION", Model2DRotation);
        p.Add("MODEL.3D.ROTX", Model3DRotX);
        p.Add("MODEL.3D.ROTY", Model3DRotY);
        p.Add("MODEL.3D.ROTZ", Model3DRotZ);
        p.Add("MODEL.3D.DZ", Model3DDz);
        p.Add("MODEL.SNAPCOUNT", ModelSnapCount);
        p.Add("MODEL.MODELTYPE", ModelType);
    }

    public ParameterCollection ExportToParameters() {
        var parameters = new ParameterCollection();
        ExportToParameters(parameters);
        return parameters;
    }
}
