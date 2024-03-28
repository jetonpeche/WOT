using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Services.Jwts;

public sealed class JwtService : IJwtService
{
    private RSA Rsa { get; init; }
    private string Issuer { get; init; }

    public JwtService(RSA _rsa, string _issuer)
    {
        Rsa = _rsa;
        Issuer = _issuer;
    }

    public string Generer(Claim[] _tabClaim) => ConstruireJwt(_tabClaim, DateTime.UtcNow.AddHours(1));

    private string ConstruireJwt(Claim[] _tabClaim, DateTime _expire)
    {
        var gestionnaireJwt = new JsonWebTokenHandler();

        // permet de signer le JWT
        var cle = new RsaSecurityKey(Rsa);

        // creation du JWT
        // 2 min de validité
        var jwt = gestionnaireJwt.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(_tabClaim),

            Issuer = Issuer,
            Expires = _expire,
            SigningCredentials = new SigningCredentials(cle, SecurityAlgorithms.RsaSha256)
        });

        return jwt!;
    }
}