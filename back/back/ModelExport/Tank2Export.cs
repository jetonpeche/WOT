using System.Text.Json.Serialization;

namespace back.ModelExport;

public sealed record Tank2Export
{
    public required int Id { get; init; }
    public required string Nom { get; init; }
    public required string NomTier { get; init; }
    public required string NomStatut { get; init; }
    public required string NomType { get; init; }
}

[JsonSerializable(typeof(Tank2Export[]))]
public partial class Tank2ExportContext: JsonSerializerContext { }
