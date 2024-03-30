namespace Services.Protections;

/// <summary>
/// Configuration du chiffrement et déchiffrement
/// </summary>
public sealed class ProtectionOptions
{
    private string cleSecret = null!;
    private string iVsecret = null!;

    /// <summary>
    /// Doit contenir 32 caractères minimum
    /// </summary>
    public required string CleSecrete
    {
        get => cleSecret;
        set
        {
            if (value.Length < 32)
                throw new ArgumentException($"{nameof(CleSecrete)} doit être de 32 caractères minimum");

            cleSecret = value;
        }
    }

    /// <summary>
    /// Doit contenir 16 caractères minimum
    /// </summary>
    public required string IVsecret
    {
        get => iVsecret;
        set
        {
            if (value.Length < 16)
                throw new ArgumentException($"{nameof(IVsecret)} doit être de 16 caractères minimum");

            iVsecret = value;
        }
    }
}
