using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;
using zoft.MauiExtensions.Core.Exceptions;
using zoft.MauiExtensions.Core.Extensions;

namespace zoft.MauiExtensions.Core.Services
{
    /// <summary>
    /// Localization service that uses <see cref="ResourceManager"/> as the source for the text
    /// </summary>
    /// <seealso cref="ILocalizationService" />
    public sealed class ResourceManagerLocalizationService : ILocalizationService
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ResourceManager _resourceManager;

        private CultureInfo _currentCulture;
        /// <summary>
        /// Gets the current culture being applied in the service when determining the text to show
        /// </summary>
        /// <value>
        /// The current culture.
        /// </value>
        public CultureInfo CurrentCulture => _currentCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerLocalizationService"/> class.
        /// </summary>
        /// <param name="resourceFileNamespace">The resource file namespace.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        public ResourceManagerLocalizationService(string resourceFileNamespace, Assembly resourceAssembly)
            : this(new ResourceManager(resourceFileNamespace.ThrowIfNull(nameof(resourceFileNamespace)),
                                       resourceAssembly.ThrowIfNull(nameof(resourceAssembly))))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerLocalizationService"/> class.
        /// </summary>
        /// <param name="resourceFileNamespace">The resource file namespace.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        /// <param name="languageCultureName">Name of the language culture.</param>
        public ResourceManagerLocalizationService(string resourceFileNamespace, Assembly resourceAssembly, string languageCultureName)
            : this(new ResourceManager(resourceFileNamespace.ThrowIfNull(nameof(resourceFileNamespace)),
                                       resourceAssembly.ThrowIfNull(nameof(resourceAssembly))),
                   languageCultureName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerLocalizationService"/> class.
        /// </summary>
        /// <param name="resourceFileNamespace">The resource file namespace.</param>
        /// <param name="resourceAssembly">The resource assembly.</param>
        /// <param name="languageCultureInfo">The language culture information.</param>
        public ResourceManagerLocalizationService(string resourceFileNamespace, Assembly resourceAssembly, CultureInfo languageCultureInfo)
            : this(new ResourceManager(resourceFileNamespace.ThrowIfNull(nameof(resourceFileNamespace)),
                                       resourceAssembly.ThrowIfNull(nameof(resourceAssembly))),
                   languageCultureInfo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerLocalizationService"/> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        public ResourceManagerLocalizationService(ResourceManager resourceManager)
            : this(resourceManager, CultureInfo.CurrentUICulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerLocalizationService"/> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="languageCultureName">Name of the language culture.</param>
        public ResourceManagerLocalizationService(ResourceManager resourceManager, string languageCultureName)
        {
            _resourceManager = resourceManager.ThrowIfNull(nameof(resourceManager));

            SetLanguage(languageCultureName.ThrowIfNull(nameof(languageCultureName)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagerLocalizationService"/> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="languageCultureInfo">The language culture information.</param>
        public ResourceManagerLocalizationService(ResourceManager resourceManager, CultureInfo languageCultureInfo)
        {
            _resourceManager = resourceManager.ThrowIfNull(nameof(resourceManager));

            SetLanguage(languageCultureInfo.ThrowIfNull(nameof(languageCultureInfo)));
        }

        /// <summary>
        /// Gets the localized text associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetTextForKey(string key)
        {
            return _resourceManager.GetString(key, _currentCulture);
        }

        /// <summary>
        /// Sets the language to use in the service.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <param name="throwIfFail">if set to <c>true</c> [throw if fail].</param>
        /// <exception cref="LocalizationLanguageNotSupported">
        /// Language not supported ({cultureName})
        /// or
        /// Language not supported ({cultureName})
        /// </exception>
        public void SetLanguage(string cultureName, bool throwIfFail = false)
        {
            CultureInfo newLanguageCultureInfo;
            try
            {
                newLanguageCultureInfo = CultureInfo.GetCultureInfo(cultureName);
                if (throwIfFail && newLanguageCultureInfo == null)
                {
                    throw new LocalizationLanguageNotSupported($"Language not supported ({cultureName})");
                }
            }
            catch (Exception ex)
            {
                if (throwIfFail)
                {
                    throw new LocalizationLanguageNotSupported($"Language not supported ({cultureName})", ex);
                }

                newLanguageCultureInfo = default;
            }

            SetLanguage(newLanguageCultureInfo);
        }

        /// <summary>
        /// Sets the language to use in the service.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="throwIfFail">if set to <c>true</c> [throw if fail].</param>
        public void SetLanguage(CultureInfo culture, bool throwIfFail = false)
        {
            _currentCulture = culture;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        /// <summary>
        /// Gets the <see cref="string"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="string"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string this[string key] => GetTextForKey(key);
    }
}
