using System.Text.Json.Serialization;

namespace back.ModelExport;

public sealed record ClanWarDetailExport
{
    public required int Id { get; init; }
    public required string Date { get; init; }
    public required ClanWarParticipant[] ListePersonne {  get; init; }
}

public sealed record ClanWarParticipant
{
    public required int Id { get; init; }
    public required string Pseudo { get; init; }
    public required string NomTank { get; init; }
}

[JsonSerializable(typeof(ClanWarDetailExport))]
public partial class ClanWarDetailExportContext: JsonSerializerContext { }
