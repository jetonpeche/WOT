using System.Text.Json.Serialization;

namespace BotDiscord.Models;

public sealed record ClanWar
{
    public required string Date { get; init; }
    public bool Participe { get; init; }
    public int NbParticipant { get; init; }
}

[JsonSerializable(typeof(ClanWar))]
[JsonSerializable(typeof(ClanWar[]))]
public partial class ClanWarContext: JsonSerializerContext { }
