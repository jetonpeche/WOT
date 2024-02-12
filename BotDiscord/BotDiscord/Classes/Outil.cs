using System.Text.RegularExpressions;

namespace BotDiscord.Classes;

public class Outil
{
    private static string PatternDate { get; } = @"^(?:(?:31(?!(\/)(?:0[13578]|1[02]))|(?:29|30)(?!(\/)(?:0[13456789]|1[12]))|(?:0[1-9]|1[0-9]|2[0-8]))(\/)(?:0[1-9]|1[0-2])(?:(\/)\d{4})?)$";

    
    public static bool FormatDateOK(string _date)
    {
        return Regex.IsMatch(_date, PatternDate);
    }

}
