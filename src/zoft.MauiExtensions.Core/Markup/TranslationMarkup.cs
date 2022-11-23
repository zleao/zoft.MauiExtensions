using zoft.MauiExtensions.Core.Services;

namespace zoft.MauiExtensions.Core.Markup
{
    [ContentProperty(nameof(Name))]
    public sealed class Translate : IMarkupExtension<BindingBase>
    {
        public string Name { get; set; }
        public IValueConverter Converter { get; set; }

        public BindingBase ProvideValue(IServiceProvider serviceProvider)
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Name}]",
                Source = ServiceHelper.GetService<ILocalizationService>(),
                Converter = Converter
            };
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }


    public static class ServiceHelper
    {

        public static T GetService<T>() => Current.GetService<T>();

        public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
    MauiWinUIApplication.Current.Services;
#elif ANDROID
    MauiApplication.Current.Services;
#elif IOS || MACCATALYST
    MauiUIApplicationDelegate.Current.Services;
#else
            null;
#endif 
    }
}
