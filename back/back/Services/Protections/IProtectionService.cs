namespace Services.Protections;

public interface IProtectionService
{
    /// <summary>
    /// Supprime toutes les balises HTML du texte
    /// </summary>
    /// <param name="_texte">texte a vérifier</param>
    /// <returns>Le texte sans balise HTML</returns>
    string XSS(string _texte);

    /// <summary>
    /// Verifie que le fichier est bien correct
    /// </summary>
    /// <param name="_fichier">fichier a vérifier</param>
    /// <param name="_poidsMax">poids max du fichier autorisé</param>
    /// <param name="_listeTypeMime">Liste des type mime autorisé</param>
    /// <param name="_listeExtension">Liste des extensions autorisé</param>
    /// <returns></returns>
    ReponseProtectionImage Fichier(IFormFile _fichier, int _poidsMax, string[] _listeTypeMime, string[] _listeExtension);

    /// <summary>
    /// Chiffrer un texte avec AES256
    /// </summary>
    /// <typeparam name="T">SI string => renvoie en HEXA / SI byte[] => renvoie en byte[]</typeparam>
    /// <param name="_texte">Texte à chiffrer</param>
    /// <param name="_cleSecrete">Clé secrete de chiffrement</param>
    /// <param name="_IVsecret">IV secret</param>
    /// <returns>Tableau de byte ou HEXA (string) du texte chiffré</returns>
    /// <exception cref="ArgumentException"></exception>
    T Chiffrer<T>(string _texte, string _cleSecrete, string _IVsecret);

    /// <summary>
    /// A utiliser si ProtectionOptions est configuré
    /// Chiffrer un texte avec AES256
    /// </summary>
    /// <typeparam name="T">SI string => renvoie en HEXA / SI byte[] => renvoie en byte[]</typeparam>
    /// <param name="_texte">Texte à chiffrer</param>
    /// <returns>Tableau de byte ou HEXA (string) du texte chiffré</returns>
    /// <exception cref="ArgumentException"></exception>
    public T Chiffrer<T>(string _texte);

    /// <summary>
    /// A utiliser si ProtectionOptions est Configuré
    /// Déchiffrer un texte sous forme de byte avec AES256
    /// </summary>
    /// /// <typeparam name="T">Doit etre type string ou byte[]</typeparam>
    /// <param name="_texteChiffrer"></param>
    /// <returns>Le texte déchiffré</returns>
    /// <exception cref="ArgumentException"></exception>
    string Dechiffrer<T>(T _texteChiffrer);

    /// <summary>
    /// Déchiffrer un texte sous forme de byte avec AES256
    /// La clé secrete et IV secret doivent être LES MEME QUE UTILISER QUE LE CHIFFREMENT
    /// </summary>
    /// /// <typeparam name="T">Doit etre type string ou byte[]</typeparam>
    /// <param name="_texteChiffrer"></param>
    /// <param name="_cleSecrete">Clé secrete de chiffrement</param>
    /// <param name="_IVsecret">IV secret</param>
    /// <returns>Le texte déchiffré</returns>
    /// <exception cref="ArgumentException"></exception>
    string Dechiffrer<T>(T _texteChiffrer, string _cleSecrete, string _IVsecret);

    /// <summary>
    /// Convertir de l'héxadecimal en byte
    /// </summary>
    /// <param name="_texteHexa">texte à convertir</param>
    /// <returns>le texte convertie en byte</returns>
    byte[] ConvertirHexaEnByte(string _texteHexa);

    /// <summary>
    /// Convertir le texte en byte en texte normal
    /// </summary>
    /// <param name="_tableauByte">texte en byte</param>
    /// <returns>le texte convertie en texte normal</returns>
    string ConvertirByteEnHexa(byte[] _tableauByte);
}
