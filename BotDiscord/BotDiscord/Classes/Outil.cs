using System.Text.RegularExpressions;

namespace BotDiscord.Classes;

public class Outil
{
    private static string PatternDate { get; } = @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))$";

    public static bool FormatDateOK(string _date)
    {
        return Regex.IsMatch(_date, PatternDate);
    }

}
