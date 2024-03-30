namespace back.Services.Fichiers;

public class FichierService : IFichierService
{
    public async Task SauvegarderAsync(string _chemin, IFormFile _fichier)
    {
        if (!Directory.Exists(_chemin))
            Directory.CreateDirectory(_chemin);

        using MemoryStream memoryStream = new();

        // recupere le fichier dans memoryStream
        await _fichier.CopyToAsync(memoryStream);

        // remet la tete de lecture au début du fichier
        memoryStream.Seek(0, SeekOrigin.Begin);

        // enregiste le fichier
        File.WriteAllBytes(Path.Combine(_chemin, _fichier.FileName), memoryStream.ToArray());
    }

    public void Supprimer(string _chemin)
    {
        File.Delete(_chemin);
    }
}
