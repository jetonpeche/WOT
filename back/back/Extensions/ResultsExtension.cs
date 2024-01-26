using System.Text.Json.Serialization;

namespace back.Extensions;

public static class ResultsExtension
{
    /// <summary>
    /// Renvoie un 200 OK sans utiliser la reflexion
    /// </summary>
    /// <param name="result"></param>
    /// <param name="_infos">infos à retourner de l'API</param>
    /// <param name="_context">context de serialization de la class</param>
    /// <returns></returns>
    public static IResult OK(this IResultExtensions result, object _infos, JsonSerializerContext _context)
    {
        return Results.Json(_infos, _context, statusCode: StatusCodes.Status200OK);
    }

    /// <summary>
    /// Generer un code http 503
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static IResult ErreurConnexionBdd(this IResultExtensions result)
    {
        return Results.Json("Allume la bdd", statusCode: StatusCodes.Status503ServiceUnavailable);
    }
}
