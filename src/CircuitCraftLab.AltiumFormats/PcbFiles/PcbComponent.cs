using System.Collections;

using CircuitCraftLab.AltiumFormats.UnitsSystem;

using static CircuitCraftLab.AltiumFormats.UnitsSystem.UnitsConverter;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public class PcbComponent : IComponent, IEnumerable<PcbPrimitive> {
    public string Pattern { get; set; } = string.Empty!;

    public string Description { get; set; } = string.Empty!;

    public Coordinate Height { get; set; }

    public string ItemGuid { get; set; } = string.Empty!;

    public string RevisionGuid { get; set; } = string.Empty!;

    string IComponent.Name => Pattern;

    string IComponent.Description => Description;

    public int Pads => Primitives.Where(p => p is PcbPad).Count();

    public List<PcbPrimitive> Primitives { get; } = new List<PcbPrimitive>();

    public IEnumerable<T> GetPrimitivesOfType<T>(bool flatten) where T : Primitive =>
        Primitives.OfType<T>();

    public CoordinateRectangular CalculateBounds() =>
        CoordinateRectangular.Union(GetPrimitivesOfType<Primitive>(true).Select(p => p.CalculateBounds()));

    public void ImportFromParameters(ParameterCollection p) {
        if (p == null) {
            throw new ArgumentNullException(nameof(p));
        }

        Pattern = p["PATTERN"].AsStringOrDefault()!;
        Height = p["HEIGHT"].AsCoordinate();
        Description = p["DESCRIPTION"].AsStringOrDefault()!;
        ItemGuid = p["ITEMGUID"].AsStringOrDefault()!;
        RevisionGuid = p["REVISIONGUID"].AsStringOrDefault()!;
    }

    public void ExportToParameters(ParameterCollection p) {
        if (p == null) {
            throw new ArgumentNullException(nameof(p));
        }

        p.Add("PATTERN", Pattern);
        p.Add("HEIGHT", Height, false);
        p.Add("DESCRIPTION", Description, false);
        p.Add("ITEMGUID", ItemGuid, false);
        p.Add("REVISIONGUID", RevisionGuid, false);
    }

    public ParameterCollection ExportToParameters() {
        var parameters = new ParameterCollection();
        ExportToParameters(parameters);
        return parameters;
    }

    public void Add(PcbPrimitive primitive) {
        if (primitive is PcbPad pad) {
            pad.Designator ??= GenerateDesignator(GetPrimitivesOfType<PcbPad>(false)
                .Select(p => p.Designator));
        } else if (primitive is PcbMetaTrack metaTrack) {
            foreach (var line in metaTrack.Lines) {
                Primitives.Add(new PcbTrack {
                    Layer = metaTrack.Layer,
                    Flags = metaTrack.Flags,
                    Start = line.Item1,
                    End = line.Item2
                });
            }
            return;
        }

        Primitives.Add(primitive);
    }

    IEnumerator<PcbPrimitive> IEnumerable<PcbPrimitive>.GetEnumerator() =>
        Primitives.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        Primitives.GetEnumerator();
}
