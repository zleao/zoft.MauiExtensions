using zoft.MauiExtensions.Core.Services;

namespace zoft.MauiExtensions.Core.Markup
{
    /// <summary>
    /// Extensions Markup that provides a transaltion service, bsed on the <see cref="ILocalizationService"/>
    /// </summary>
    [ContentProperty(nameof(Name))]
    public sealed class Translate : IMarkupExtension<BindingBase>
    {
        /// <summary>
        /// Name of the key to get from the text resource source
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Converter to apply to the bound value
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Provides the BindingBase instance to apply
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>

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
}
