using System.Globalization;

namespace zoft.MauiExtensions.Sample.Localization
{
    internal static class SupportedLanguages
    {
        public static List<CultureInfo> List { get; } = new List<CultureInfo>
        {
            new CultureInfo("en"), //default
            new CultureInfo("pt"),
            new CultureInfo("de"),
        };

        public static CultureInfo DefaultLanguage => List[0];
    }
}
