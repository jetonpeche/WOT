using Microsoft.AspNetCore.StaticFiles;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Protections;

public sealed class ProtectionService: IProtectionService
{
    private readonly ProtectionOptions? option;

    public ProtectionService(ProtectionOptions? _option = null)
    {
        option = _option;
    }

    public string XSS(string _texte)
    {
        if (string.IsNullOrEmpty(_texte))
            return "";

        return Regex.Replace(_texte, "<[^>]*>", "");
    }

    public ReponseProtectionImage Fichier(IFormFile _fichier, int _poidsMax, string[] _listeTypeMime, string[] _listeExtension)
    {
        var p = new FileExtensionContentTypeProvider();
        p.TryGetContentType(_fichier.FileName, out string typeMime);

        if (_fichier.Length > _poidsMax)
            return new ReponseProtectionImage
            {
                Message = $"Le poids du fichier {_fichier.FileName} ne peut pas être plus grand que: {_poidsMax}"
            };


        // verifie le type mime de l'image
        if (!_listeTypeMime.Contains(typeMime))
            return new ReponseProtectionImage { Message = $"Le format du fichier {_fichier.FileName} n'ai pas autorisé" };

        // verifie l'extention
        if (!_listeExtension.Contains(Path.GetExtension(_fichier.FileName)))
            return new ReponseProtectionImage { Message = $"extension incorrect du fichier {_fichier.FileName}" };

        return new ReponseProtectionImage
        {
            EstOK = true,
            Message = "ok"
        };
    }

    public T Chiffrer<T>(string _texte, string _cleSecrete, string _IVsecret)
    {
        ControlerTypeTParametre(typeof(T));

        return ChiffrerPriver<T>(_texte, _cleSecrete, _IVsecret);
    }

    public T Chiffrer<T>(string _texte)
    {
        if (option is null)
            throw new ArgumentNullException($"Pour utiliser cette variante configurer {nameof(ProtectionOptions)}");

        ControlerTypeTParametre(typeof(T));

        if (string.IsNullOrEmpty(_texte) || string.IsNullOrWhiteSpace(_texte))
            throw new ArgumentException($"{nameof(_texte)} est vide ou null");

        return ChiffrerPriver<T>(_texte, option.CleSecrete, option.IVsecret);
    }

    public string Dechiffrer<T>(T _texteChiffrer, string _cleSecrete, string _IVsecret)
    {
        ControlerTypeTParametre(typeof(T));

        if (_cleSecrete.Length < 32)
            throw new ArgumentException($"{nameof(_cleSecrete)} doit être d'au moins de 32 caractères");

        if (_IVsecret.Length < 16)
            throw new ArgumentException($"{nameof(_IVsecret)} doit être d'au moins de 16 caractères");

        return DechiffrerPriver(_texteChiffrer, _cleSecrete, _IVsecret);
    }

    public string Dechiffrer<T>(T _texteChiffrer)
    {
        if (option is null)
            throw new ArgumentNullException($"Pour utiliser cette variante configurer {nameof(ProtectionOptions)}");

        ControlerTypeTParametre(typeof(T));

        return DechiffrerPriver(_texteChiffrer, option.CleSecrete, option.IVsecret);
    }

    public byte[] ConvertirHexaEnByte(string _texteHexa)
    {
        if (string.IsNullOrWhiteSpace(_texteHexa))
            throw new ArgumentException($"{nameof(_texteHexa)} est vide ou null");

        int longeurTableau = _texteHexa.Length / 2;

        // 1 byte = 8 bits
        // 1 hex = 16 bits
        // => diviser par 2
        var tableauByteRetour = new byte[longeurTableau];

        // convertie la pair HEXA en BYTE
        for (int i = 0; i < longeurTableau; i++)
            tableauByteRetour[i] = Convert.ToByte(_texteHexa.Substring(i * 2, 2), 16);

        return tableauByteRetour;
    }

    public string ConvertirByteEnHexa(byte[] _tableauByte)
    {
        if (_tableauByte is null || _tableauByte.Length == 0)
            throw new ArgumentException("Le tableau est vide ou null");

        return BitConverter.ToString(_tableauByte).Replace("-", "");
    }

    private void ControlerTypeTParametre(Type _typeT)
    {
        if (_typeT.Name != typeof(string).Name && _typeT.Name != typeof(byte[]).Name)
            throw new ArgumentException("Type T doit etre un type string ou byte[]");
    }

    private T ChiffrerPriver<T>(string _texte, string _cleSecrete, string _IVsecret)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_cleSecrete.Substring(0, 32));
            aes.IV = Encoding.UTF8.GetBytes(_IVsecret.Substring(0, 16));

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            // Create the streams used for encryption.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))

                    streamWriter.Write(_texte);

                if (typeof(T) == typeof(string))
                {
                    var retour = (T)(object)ConvertirByteEnHexa(memoryStream.ToArray());
                    return retour;
                }

                return (T)(object)memoryStream.ToArray();
            }
        }
    }

    private string DechiffrerPriver<T>(T _texteChiffrer, string _cleSecrete, string _IVsecret)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_cleSecrete.Substring(0, 32));
            aes.IV = Encoding.UTF8.GetBytes(_IVsecret.Substring(0, 16));

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] tableauTempo = Array.Empty<byte>();

            if (typeof(T) == typeof(string))
                tableauTempo = ConvertirHexaEnByte((string)(object)_texteChiffrer!);
            else
                tableauTempo = (byte[])(object)_texteChiffrer!;

            using (MemoryStream memoryStream = new MemoryStream(tableauTempo))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))

                    return streamReader.ReadToEnd();
            }
        }
    }
}
