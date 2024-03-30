namespace back.Services.Fichiers;

public interface IFichierService
{
    /// <summary>
    /// Sauvegarder le fichier
    /// </summary>
    /// <param name="_chemin">chemin de destination du fichier</param>
    /// <param name="_fichier">fichier conserné</param>
    Task SauvegarderAsync(string _chemin, IFormFile _fichier);

    /// <summary>
    /// Supprimer un fichier
    /// </summary>
    /// <param name="_chemin">chemin du fichier</param>
    /// <returns></returns>
    void Supprimer(string _chemin);
}
