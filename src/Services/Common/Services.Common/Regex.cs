using System.Text.RegularExpressions;

namespace Services.Common;



public static partial class UrlFormatChecker
{
    private const string Pattern = @"^(https)://[^\s/$.?#].[^\s]*$";

    [GeneratedRegex(Pattern, RegexOptions.IgnoreCase, "en-US")]
    public static partial Regex UrlRegex();
}
