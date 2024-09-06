using System.ComponentModel;
using System.Globalization;

namespace zoft.MauiExtensions.Core.Services;

/// <summary>
/// Generic interface for a service that allows localization of an app
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
public interface ILocalizationService : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the current culture being applied in the service when determining the text to show
    /// </summary>
    /// <value>
    /// The current culture.
    /// </value>
    CultureInfo CurrentCulture { get; }

    /// <summary>
    /// Sets the language to use in the service.
    /// </summary>
    /// <param name="cultureName">Name of the culture.</param>
    /// <param name="throwIfFail">if set to <c>true</c> [throw if fail].</param>
    void SetLanguage(string cultureName, bool throwIfFail = false);

    /// <summary>
    /// Sets the language to use in the service.
    /// </summary>
    /// <param name="culture">The culture.</param>
    /// <param name="throwIfFail">if set to <c>true</c> [throw if fail].</param>
    void SetLanguage(CultureInfo culture, bool throwIfFail = false);

    /// <summary>
    /// Gets the localized text associated with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    string GetTextForKey(string key);

    /// <summary>
    /// Gets the <see cref="string"/> with the specified key.
    /// </summary>
    /// <value>
    /// The <see cref="string"/>.
    /// </value>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    string this[string key] { get; }
}
