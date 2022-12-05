using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace back.Services;
public class TokenService
{
	private IConfiguration Config { get; init; }

	public TokenService(IConfiguration _config)
	{
		Config = _config;
	}

    public string Generer(int _idCompte)
    {
        var cle = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetValue<string>("Token:cleSecrete")));
        var cleSigner = new SigningCredentials(cle, SecurityAlgorithms.HmacSha256);

        // ajout des infos divers dans le token
        Claim[] claim = new[]
        {
            new Claim("idCompte", _idCompte.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: Config.GetValue<string>("Token:issuer"),
            audience: Config.GetValue<string>("Token:audience"),
            claims: claim,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: cleSigner
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
