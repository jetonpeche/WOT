namespace Services.Protections;

public sealed record ReponseProtectionImage
{
    public bool EstOK { get; init; }
    public required string Message { get; init; } = null!;
}
