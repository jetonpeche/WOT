using System.Text.Json.Serialization;

namespace back.ModelExport;

public sealed record JoueurExport
{
    public required int Id { get; init; }
    public required string IdDiscord { get; init; }
    public required string Pseudo { get; init; }
    public required bool EstAdmin { get; init; }
    public required bool EstStrateur { get; init; }
    public required bool EstActiver { get; init; }
    public required int[] ListeIdTank { get; init; }
}

[JsonSerializable(typeof(JoueurExport[]))]
[JsonSerializable(typeof(JoueurExport))]
public partial class JoueurExportContext: JsonSerializerContext { }
