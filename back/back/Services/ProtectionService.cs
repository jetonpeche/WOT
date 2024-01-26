using System.Text.RegularExpressions;

namespace back.Services;

public sealed class ProtectionService
{
    public string XSS(string _text)
    {
        if (string.IsNullOrEmpty(_text))
            return "";

        Regex regHtml = new Regex("<[^>]*>");
        return regHtml.Replace(_text, "");
    }
}
