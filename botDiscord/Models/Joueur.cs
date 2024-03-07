using System.Text.Json.Serialization;

namespace BotDiscord.Models;

public sealed record Joueur
{
    public int Id { get; init; }

    public required string IdDiscord { get; init; }
    public required string Pseudo { get; init; }

    public bool EstStrateur { get; init; }
    public bool EstAdmin { get; init; }
    public bool EstActiver { get; init; }
}

[JsonSerializable(typeof(Joueur))]
[JsonSerializable(typeof(Joueur[]))]
public partial class JoueurContext: JsonSerializerContext { }
