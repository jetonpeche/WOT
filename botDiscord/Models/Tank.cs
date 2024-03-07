using System.Text.Json.Serialization;

namespace BotDiscord.Models;

public sealed record Tank
{
    public int Id { get; init; }

    public required string Nom { get; init; }
    public required string NomTier { get; init; }
    public required string NomStatut { get; init; }
    public required string NomType { get; init; }
}

[JsonSerializable(typeof(Tank))]
[JsonSerializable(typeof(Tank[]))]
public partial class TankContext: JsonSerializerContext { }
