using OpenMcdf;

using CircuitCraftLab.AltiumFormats.CompoundFiles;
using CircuitCraftLab.AltiumFormats.UnitsSystem;
using System.Text;

namespace CircuitCraftLab.AltiumFormats.PcbFiles;

public sealed class PcbLibReader : CompoundFileReader<PcbLib> {
    public PcbLibReader() : base() {
    }

    protected override void DoRead() {
        ReadFileHeader();
        ReadSectionKeys();
        ReadLibrary();
    }

    private void ReadSectionKeys() {
        SectionKeys.Clear();

        var data = CompoundFile.TryGetStream("SectionKeys");
        if (data == null) {
            return;
        }

        BeginContext("SectionKeys");

        using (var reader = data.GetBinaryReader()) {
            var keyCount = reader.ReadInt32();
            for (var i = 0; i < keyCount; ++i) {
                var libRef = ReadPascalString(reader);
                var sectionKey = ReadStringBlock(reader);
                SectionKeys.Add(libRef, sectionKey);
            }
        }

        EndContext();
    }

    private PcbComponent ReadFootprint(string sectionKey) {
        var footprintStorage = CompoundFile.TryGetStorage(sectionKey) ?? throw new ArgumentException($"Footprint resource not found: {sectionKey}");

        BeginContext(sectionKey);

        var recordCount = ReadHeader(footprintStorage);

        var component = new PcbComponent();
        ReadFootprintParameters(footprintStorage, component);

        var wideStrings = ReadWideStrings(footprintStorage);

        using (var reader = footprintStorage.GetStream("Data").GetBinaryReader()) {
            AssertValue(nameof(component.Pattern), component.Pattern, ReadStringBlock(reader));

            var ndxRecord = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length) {
                if (ndxRecord > recordCount) {
                    EmitWarning("Number of existing records exceed the header's record count");
                }

                // save the stream position so we can later recover the raw component data
                var primitiveStartPosition = reader.BaseStream.Position;
                var objectId = (PcbPrimitiveObjectId) reader.ReadByte();
                BeginContext(objectId.ToString());
                PcbPrimitive element;
                switch (objectId) {
                    case PcbPrimitiveObjectId.Arc:
                        element = ReadFootprintArc(reader);
                        break;

                    case PcbPrimitiveObjectId.Pad:
                        element = ReadFootprintPad(reader);
                        break;

                    case PcbPrimitiveObjectId.Via:
                        element = ReadFootprintVia(reader);
                        break;

                    case PcbPrimitiveObjectId.Track:
                        element = ReadFootprintTrack(reader);
                        break;

                    case PcbPrimitiveObjectId.Text:
                        element = ReadFootprintString(reader, wideStrings);
                        break;

                    case PcbPrimitiveObjectId.Fill:
                        element = ReadFootprintFill(reader);
                        break;

                    case PcbPrimitiveObjectId.Region:
                        element = ReadFootprintRegion(reader);
                        break;

                    case PcbPrimitiveObjectId.ComponentBody:
                        element = ReadFootprintComponentBody(reader);
                        break;

                    default:
                        element = ReadFootprintUknown(reader, objectId);
                        break;
                }

                element.SetRawData(ExtractStreamData(reader, primitiveStartPosition, reader.BaseStream.Position));

                component.Primitives.Add(element);

                EndContext();
                ndxRecord++;
            }
        }

        ReadUniqueIdPrimitiveInformation(footprintStorage, component);

        EndContext();

        return component;
    }

    private void ReadFootprintParameters(CFStorage componentStorage, PcbComponent component) {
        BeginContext("Parameters");
        try {
            using var reader = componentStorage.GetStream("Parameters").GetBinaryReader();
            var parameters = ReadBlock(reader, size => ReadParameters(reader, size));
            component.ImportFromParameters(parameters);
        } finally {
            EndContext();
        }
    }

    private void ReadUniqueIdPrimitiveInformation(CFStorage componentStorage, PcbComponent component) {
        if (!componentStorage.TryGetStorage("UniqueIdPrimitiveInformation", out var uniqueIdPrimitiveInformation)) return;

        BeginContext("UniqueIdPrimitiveInformation");
        try {
            var recordCount = ReadHeader(uniqueIdPrimitiveInformation);
            using var reader = uniqueIdPrimitiveInformation.GetStream("Data").GetBinaryReader();
            uint actualRecordCount = 0;
            while (reader.BaseStream.Position < reader.BaseStream.Length) {
                var parameters = ReadBlock(reader, size => ReadParameters(reader, size));
                var primitiveIndex = parameters["PRIMITIVEINDEX"].AsIntOrDefault();
                var primitiveObjectId = parameters["PRIMITIVEOBJECTID"].AsStringOrDefault();
                var uniqueId = parameters["UNIQUEID"].AsStringOrDefault();

                if (!CheckValue("PRIMITIVEINDEX < Primitives.Count", primitiveIndex < component.Primitives.Count, true)) {
                    return;
                }

                var primitive = component.Primitives[primitiveIndex];

                if (!CheckValue(nameof(primitiveObjectId)!, primitiveObjectId!, primitive.ObjectId.ToString()!)) {
                    return;
                }

                primitive.UniqueId = uniqueId!;
                actualRecordCount++;
            }
            AssertValue(nameof(actualRecordCount), actualRecordCount, recordCount);
        } finally {
            EndContext();
        }
    }


    private void Assert10FFbytes(BinaryReader reader) {
        AssertValue("10 0xFF bytes", reader.ReadBytes(10).All(b => b == 0xFF), true);
    }

    private static PcbPrimitive ReadFootprintUknown(BinaryReader reader, PcbPrimitiveObjectId objectId) {
        ReadBlock(reader);
        return new PcbUnknown(objectId);
    }

    private void ReadFootprintCommon(BinaryReader reader, PcbPrimitive primitive) {
        primitive.Layer = reader.ReadByte();
        primitive.Flags = (PcbFlags) reader.ReadUInt16();
        Assert10FFbytes(reader);
    }

    private PcbArc ReadFootprintArc(BinaryReader reader) {
        return ReadBlock(reader, recordSize => {
            CheckValue(nameof(recordSize), recordSize, 45, 56);
            var arc = new PcbArc();
            ReadFootprintCommon(reader, arc);
            arc.Location = ReadCoordPoint(reader);
            arc.Radius = reader.ReadInt32();
            arc.StartAngle = reader.ReadDouble();
            arc.EndAngle = reader.ReadDouble();
            arc.Width = reader.ReadInt32();
            if (recordSize >= 56) {
                reader.ReadUInt32();
                reader.ReadUInt16();
                reader.ReadByte();
                reader.ReadUInt32();
            }
            return arc;
        });
    }

    private PcbPad ReadFootprintPad(BinaryReader reader) {
        var pad = new PcbPad {
            Designator = ReadStringBlock(reader)
        };

        ReadBlock(reader);
        ReadStringBlock(reader);
        ReadBlock(reader);

        ReadBlock(reader, blockSize => {
            ReadFootprintCommon(reader, pad);
            pad.Location = ReadCoordPoint(reader);
            pad.SizeTop = ReadCoordPoint(reader);
            pad.SizeMiddle = ReadCoordPoint(reader);
            pad.SizeBottom = ReadCoordPoint(reader);
            pad.HoleSize = reader.ReadInt32();
            pad.ShapeTop = (PcbPadShape) reader.ReadByte();
            pad.ShapeMiddle = (PcbPadShape) reader.ReadByte();
            pad.ShapeBottom = (PcbPadShape) reader.ReadByte();
            pad.Rotation = reader.ReadDouble();
            pad.IsPlated = reader.ReadBoolean();
            CheckValue("#91", reader.ReadByte(), 0);
            pad.StackMode = (PcbStackMode) reader.ReadByte();
            reader.ReadByte();
            reader.ReadInt32();
            reader.ReadInt32();
            CheckValue("#102", reader.ReadInt16(), 4);
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            pad.PasteMaskExpansion = Coordinate.FromInt32(reader.ReadInt32());
            pad.SolderMaskExpansion = Coordinate.FromInt32(reader.ReadInt32());
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            pad.PasteMaskExpansionManual = reader.ReadByte() == 2;
            pad.SolderMaskExpansionManual = reader.ReadByte() == 2;
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadUInt32();
            pad.JumperId = reader.ReadInt16();
            reader.ReadInt16();
        });

        ReadBlock(reader, blockSize => {
            CheckValue(nameof(blockSize), blockSize, 596, 628);
            var padXSizes = new Coordinate[29];
            var padYSizes = new Coordinate[29];

            for (var i = 0; i < 29; ++i) {
                padXSizes[i] = reader.ReadInt32();
            }
            for (var i = 0; i < 29; ++i) {
                padYSizes[i] = reader.ReadInt32();
            }

            for (var i = 0; i < 29; ++i) {
                pad.SizeMiddleLayers[i] = new CoordinatePoint(padXSizes[i], padYSizes[i]);
            }

            for (var i = 1; i < 30; ++i) {
                pad.ShapeLayers[i] = (PcbPadShape) reader.ReadByte();
            }

            reader.ReadByte();
            pad.HoleShape = (PcbPadHoleShape) reader.ReadByte();
            pad.HoleSlotLength = reader.ReadInt32();
            pad.HoleRotation = reader.ReadDouble();

            var offsetXFromHoleCenter = new int[32];
            var offsetYFromHoleCenter = new int[32];
            for (var i = 0; i < 32; ++i) {
                offsetXFromHoleCenter[i] = reader.ReadInt32();
            }
            for (var i = 0; i < 32; ++i) {
                offsetYFromHoleCenter[i] = reader.ReadInt32();
            }

            for (var i = 0; i < 32; ++i) {
                pad.OffsetsFromHoleCenter[i] = new CoordinatePoint(offsetXFromHoleCenter[i], offsetYFromHoleCenter[i]);
            }

            var hasRoundedRect = reader.ReadBoolean();
            if (hasRoundedRect) {
                for (var i = 0; i < 32; ++i) pad.ShapeLayers[i] = (PcbPadShape) reader.ReadByte();
            } else {
                for (var i = 0; i < 32; ++i) reader.ReadByte();
            }
            for (var i = 0; i < 32; ++i) {
                pad.CornerRadiusPercentage[i] = reader.ReadByte();
            }
        });

        return pad;
    }

    private PcbVia ReadFootprintVia(BinaryReader reader) {
        return ReadBlock(reader, recordSize => {
            var via = new PcbVia();
            ReadFootprintCommon(reader, via);
            via.Location = ReadCoordPoint(reader);
            via.Diameter = Coordinate.FromInt32(reader.ReadInt32());
            via.HoleSize = Coordinate.FromInt32(reader.ReadInt32());
            via.FromLayer = reader.ReadByte();
            via.ToLayer = reader.ReadByte();
            reader.ReadByte();
            via.ThermalReliefAirGapWidth = Coordinate.FromInt32(reader.ReadInt32());
            via.ThermalReliefConductors = reader.ReadByte();
            reader.ReadByte();
            via.ThermalReliefConductorsWidth = Coordinate.FromInt32(reader.ReadInt32());
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadInt32();
            via.SolderMaskExpansion = Coordinate.FromInt32(reader.ReadInt32());
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            via.SolderMaskExpansionManual = reader.ReadByte() == 2;
            reader.ReadByte();
            reader.ReadInt16();
            reader.ReadInt32();
            via.DiameterStackMode = (PcbStackMode) reader.ReadByte();
            for (var i = 0; i < 32; ++i) {
                via.Diameters[i] = Coordinate.FromInt32(reader.ReadInt32());
            }
            reader.ReadInt16();
            reader.ReadInt32();
            return via;
        });
    }

    private PcbTrack ReadFootprintTrack(BinaryReader reader) {
        return ReadBlock(reader, recordSize => {
            CheckValue(nameof(recordSize), recordSize, 36, 41, 45);
            var track = new PcbTrack();
            ReadFootprintCommon(reader, track);
            var startX = reader.ReadInt32();
            var startY = reader.ReadInt32();
            track.Start = new CoordinatePoint(startX, startY);
            var endX = reader.ReadInt32();
            var endY = reader.ReadInt32();
            track.End = new CoordinatePoint(endX, endY);
            track.Width = reader.ReadInt32();
            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            if (recordSize >= 41) {
                reader.ReadByte();
                reader.ReadUInt32();
            }
            if (recordSize >= 45) {
                reader.ReadUInt32();
            }
            return track;
        });
    }

    private PcbText ReadFootprintString(BinaryReader reader, List<string> wideStrings) {
        var result = ReadBlock(reader, recordSize => {
            CheckValue(nameof(recordSize), recordSize, 43, 123, 226, 230);
            var text = new PcbText();
            ReadFootprintCommon(reader, text);
            text.Corner1 = ReadCoordPoint(reader);
            var height = reader.ReadInt32();
            text.Corner2 = new CoordinatePoint(
                Coordinate.FromInt32(text.Corner1.X.ToInt32()),
                Coordinate.FromInt32(text.Corner1.Y.ToInt32() + height));
            text.StrokeFont = (PcbTextStrokeFont) reader.ReadInt16();
            text.Rotation = reader.ReadDouble();
            text.Mirrored = reader.ReadBoolean();
            text.StrokeWidth = reader.ReadInt32();

            if (recordSize >= 123) {
                reader.ReadUInt16();
                reader.ReadByte();
                text.TextKind = (PcbTextKind) reader.ReadByte();
                text.FontBold = reader.ReadBoolean();
                text.FontItalic = reader.ReadBoolean();
                text.FontName = ReadStringFontName(reader);
                text.BarcodeLRMargin = reader.ReadInt32();
                text.BarcodeTBMargin = reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadInt32();
                reader.ReadUInt16();
                reader.ReadInt32();
                reader.ReadInt32();
                text.FontInverted = reader.ReadBoolean();
                text.FontInvertedBorder = reader.ReadInt32();
                text.WideStringsIndex = reader.ReadInt32();
                reader.ReadInt32();
                text.FontInvertedRect = reader.ReadBoolean();
                text.FontInvertedRectWidth = reader.ReadInt32();
                text.FontInvertedRectHeight = reader.ReadInt32();
                text.FontInvertedRectJustification = (PcbTextJustification) reader.ReadByte();
                text.FontInvertedRectTextOffset = reader.ReadInt32();
            }
            return text;
        });

        var asciiText = ReadStringBlock(reader);
        if (result.WideStringsIndex < wideStrings?.Count) {
            result.Text = wideStrings[result.WideStringsIndex];
        } else {
            result.Text = asciiText;
        }
        return result;
    }

    private PcbFill ReadFootprintFill(BinaryReader reader) {
        return ReadBlock(reader, recordSize => {
            CheckValue(nameof(recordSize), recordSize, 37, 41, 46);
            var fill = new PcbFill();
            ReadFootprintCommon(reader, fill);
            fill.Corner1 = ReadCoordPoint(reader);
            fill.Corner2 = ReadCoordPoint(reader);
            fill.Rotation = reader.ReadDouble();
            if (recordSize >= 41) {
                reader.ReadUInt32();
            }
            if (recordSize >= 46) {
                reader.ReadByte();
                reader.ReadInt32();
            }
            return fill;
        });
    }

    private ParameterCollection ReadFootprintCommonParametersAndOutline(BinaryReader reader, PcbPrimitive primitive,
        List<CoordinatePoint> outline) {
        ParameterCollection parameters = null!;
        ReadBlock(reader, recordSize => {
            ReadFootprintCommon(reader, primitive);
            reader.ReadUInt32();
            reader.ReadByte();
            parameters = ReadBlock(reader, size => ReadParameters(reader, size));
            var outlineSize = reader.ReadUInt32();
            for (var i = 0; i < outlineSize; ++i) {
                Coordinate x = (int) reader.ReadDouble();
                Coordinate y = (int) reader.ReadDouble();
                outline.Add(new CoordinatePoint(x, y));
            }
        });
        return parameters;
    }

    private PcbRegion ReadFootprintRegion(BinaryReader reader) {
        var region = new PcbRegion();
        var parameters = ReadFootprintCommonParametersAndOutline(reader, region, region.Outline);
        region.Parameters = parameters;
        return region;
    }

    private PcbComponentBody ReadFootprintComponentBody(BinaryReader reader) {
        var body = new PcbComponentBody();
        var parameters = ReadFootprintCommonParametersAndOutline(reader, body, body.Outline);
        body.ImportFromParameters(parameters);
        return body;
    }

    private void ReadLibraryModels(CFStorage library) {
        BeginContext("Models");
        var models = library.GetStorage("Models");
        var recordCount = ReadHeader(models);
        using (var reader = models.GetStream("Data").GetBinaryReader()) {
            for (var i = 0; i < recordCount; ++i) {
                var parameters = ReadBlock(reader, size => ReadParameters(reader, size));
                var modelId = parameters["ID"].AsString();
                var modelCompressedData = models.GetStream($"{i}").GetData();

                var stepModel = ParseCompressedZlibData(modelCompressedData, stream => {
                    using var modelReader = new StreamReader(stream, Encoding.ASCII);
                    return modelReader.ReadToEnd();
                });

                var bodies = Data?.Items
                    .SelectMany(c => c.GetPrimitivesOfType<PcbComponentBody>(false))
                    .Where(body => body.ModelId.ToUpperInvariant() == modelId.ToUpperInvariant());
                foreach (var body in bodies!) {
                    body.StepModel = stepModel;
                }
            }
        }
        EndContext();
    }

    private void ReadLibraryData(CFStorage library) {
        using var reader = library.GetStream("Data").GetBinaryReader();
        var parameters = ReadBlock(reader, size => ReadParameters(reader, size));
        Data?.Header.ImportFromParameters(parameters);

        var footprintCount = reader.ReadUInt32();
        for (var i = 0; i < footprintCount; ++i) {
            var refName = ReadStringBlock(reader);
            var sectionKey = GetSectionKeyFromRefName(refName);
            Data?.Items.Add(ReadFootprint(sectionKey));
        }
    }

    private void ReadFileHeader() {
        var data = CompoundFile.TryGetStream("FileHeader");
        if (data == null) return;

        using var header = data.GetBinaryReader();
        var pcbBinaryFileVersionTextLength = header.ReadInt32();
        var pcbBinaryFileVersionText = ReadPascalShortString(header);
        if (header.BaseStream.Position < header.BaseStream.Length) {
            ReadPascalShortString(header);
            ReadPascalShortString(header);

            if (Data is not null) {
                Data.UniqueId = ReadPascalShortString(header);
            }
        }
    }

    private void ReadLibrary() {
        var library = CompoundFile.GetStorage("Library");
        _ = ReadHeader(library);
        ReadLibraryData(library);
        ReadLibraryModels(library);
    }
}
