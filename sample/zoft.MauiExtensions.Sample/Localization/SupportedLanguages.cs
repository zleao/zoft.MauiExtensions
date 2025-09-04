using System.Globalization;
using System.Xml.Linq;

namespace zoft.MauiExtensions.Sample.Localization
{
    internal static class SupportedLanguages
    {
        public static List<(string Name, CultureInfo Info)> List { get; } = new List<(string Name, CultureInfo Info)>
        {
            ("English", new CultureInfo("en")), //default
            ("Português", new CultureInfo("pt")),
            ("Deutsch", new CultureInfo("de")),
        };

        public static (string Name, CultureInfo Info) DefaultLanguage => List[0];
    }
}
