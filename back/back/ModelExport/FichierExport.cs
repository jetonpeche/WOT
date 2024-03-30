using System.Text.Json.Serialization;

namespace back.ModelExport;

public sealed record FichierExport
{
    public required string Nom { get; set; }
    public required string Base64 { get; set; }
}

[JsonSerializable(typeof(List<FichierExport>))]
public partial class FichierExportContext: JsonSerializerContext { }
