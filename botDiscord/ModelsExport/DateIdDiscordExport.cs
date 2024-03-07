using System.Text.Json.Serialization;

namespace botDiscord.ModelsExport;
public sealed record DateIdDiscordExport<T>
{
    public required string IdDiscord { get; init; }
    public required T Date { get; init; }
}

[JsonSerializable(typeof(DateIdDiscordExport<string>))]
[JsonSerializable(typeof(DateIdDiscordExport<DateTime>))]
public partial class DateIdDiscordExportContext : JsonSerializerContext { }
