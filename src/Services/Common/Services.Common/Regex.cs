using System.Text.RegularExpressions;

namespace Services.Common;



public static partial class UrlFormatChecker
{
    private const string Pattern = @"^(https?://)(?!www\.)[^\/.]+\.[^\/.]+$";

    [GeneratedRegex(Pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex UrlRegex();
}
