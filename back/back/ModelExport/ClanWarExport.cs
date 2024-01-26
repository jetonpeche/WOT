using System.Text.Json.Serialization;

namespace back.ModelExport;

public sealed record ClanWarExport
{
    public required int Id { get; init; }
    public required string Date { get; init; }
    public required ushort NbParticipant { get; init; }
    public required bool Participe { get; init; }
}

[JsonSerializable(typeof(ClanWarExport[]))]
[JsonSerializable(typeof(ClanWarExport))]
public partial class ClanWarExportContext: JsonSerializerContext { }
