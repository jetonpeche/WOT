using System.Security.Claims;

namespace Services.Jwts;

public interface IJwtService
{
    /// <summary>
    /// Generer un JWT
    /// </summary>
    /// <param name="_tabClaim">Infos contenu dans le JWT</param>
    /// <returns>Renvoie le JWT</returns>
    string Generer(Claim[] _tabClaim);
}