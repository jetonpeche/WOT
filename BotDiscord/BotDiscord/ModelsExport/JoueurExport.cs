using System.Text.Json.Serialization;

namespace BotDiscord.ModelsExport;

public record JoueurExport
{
    public required string IdDiscord { get; init; }
    public required string Pseudo { get; init; }
    public required bool EstStrateur { get; init; }
    public required bool EstAdmin { get; init; }
}

[JsonSerializable(typeof(JoueurExport))]
public partial class JoueurExportContext: JsonSerializerContext { }
