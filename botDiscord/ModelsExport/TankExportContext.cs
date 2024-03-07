using System.Text.Json.Serialization;

namespace BotDiscord.ModelsExport;

public sealed record TankExport
{
    public required string Nom { get; init; }
    public int IdType { get; init; }
    public int IdStatut { get; init; }
    public int IdTier { get; init; }
}

[JsonSerializable(typeof(TankExport))]
public partial class TankExportContext: JsonSerializerContext { }
