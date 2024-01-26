using System.Text.Json.Serialization;

namespace back.ModelExport;

public sealed record TankExport
{
    public required int Id { get; init; }
    public required string Nom { get; init; }
    public required int IdStatut { get; init; }
    public required int IdTypeTank { get; init; }
    public required int IdTier { get; init; }
    public required bool EstVisible { get; init; }
    public required ushort NbPossesseur { get; init; }
}

[JsonSerializable(typeof(TankExport[]))]
[JsonSerializable(typeof(TankExport))]
public partial class TankExportContext: JsonSerializerContext { }
