using System.Text.Json.Serialization;

namespace botDiscord.ModelsExport;

public sealed record IdTankDiscordExport
{
    public required int IdTank { get; init; }
    public required string IdDiscord { get; init; }
}

[JsonSerializable(typeof(IdTankDiscordExport))]
public partial class IdTankDiscordExportContext: JsonSerializerContext { }
