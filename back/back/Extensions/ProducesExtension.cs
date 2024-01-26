namespace back.Extensions;

public static class ProducesExtension
{
    /// <summary>
    /// Renvoie un code erreur 201
    /// </summary>
    public static RouteHandlerBuilder ProducesCreated(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status201Created);

    /// <summary>
    /// Renvoie un code erreur 204
    /// </summary>
    public static RouteHandlerBuilder ProducesNoContent(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status204NoContent);

    /// <summary>
    /// Renvoie un code erreur 400
    /// </summary>
    public static RouteHandlerBuilder ProducesBadRequest(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status400BadRequest);

    /// <summary>
    /// Renvoie un code erreur 404
    /// </summary>
    public static RouteHandlerBuilder ProducesNotFound(this RouteHandlerBuilder builder)
        => builder.Produces(StatusCodes.Status404NotFound);

    /// <summary>
    /// Renvoie un code erreur 503. Peut être mis sur tout les endpoints d'une route
    /// </summary>
    public static IEndpointConventionBuilder ProducesServiceUnavailable(this RouteGroupBuilder builder)
    {
        return builder.WithMetadata(new ProducesResponseTypeMetadata(
                StatusCodes.Status503ServiceUnavailable,
                typeof(string))
            );
    }
}
