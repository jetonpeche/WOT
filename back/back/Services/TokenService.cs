using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace back.Services;

public sealed class TokenService(RSA _rsa, string _issuer)
{
    public string Generer(Claim[] _tabClaim)
    {
        var gestionnaireJwt = new JsonWebTokenHandler();

        // permet de signer le JWT
        var cle = new RsaSecurityKey(_rsa);

        // creation du JWT
        // par defaut dure 1 heure
        var jwt = gestionnaireJwt.CreateToken(new SecurityTokenDescriptor
        {
            // informations ajouter dans le JWT
            Subject = new ClaimsIdentity(_tabClaim),

            // OBLIGATOIRE => qui est l'émeteur
            // en général mettre URL
            Issuer = _issuer,

            SigningCredentials = new SigningCredentials(cle, SecurityAlgorithms.RsaSha256)
        });

        return jwt;
    }
}
