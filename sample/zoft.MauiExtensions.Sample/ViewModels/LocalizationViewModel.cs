using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using zoft.MauiExtensions.Core.Attributes;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Core.ViewModels;
using zoft.MauiExtensions.Sample.Localization;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class LocalizationViewModel : CoreViewModel
    {
        public ILocalizationService LocalizationService { get; }

        public List<CultureInfo> Languages => SupportedLanguages.List;

        [ObservableProperty]
        private CultureInfo _selectedLanguage;

        public LocalizationViewModel(IMainThreadService mainThreadService, ILocalizationService localizationService) 
            : base(mainThreadService)
        {
            LocalizationService = localizationService;

            SelectedLanguage = LocalizationService.CurrentCulture;
        }

        [DependsOn(nameof(SelectedLanguage))]
        public void OnSelectedLanguageChanged()
        {
            LocalizationService.SetLanguage(SelectedLanguage);
        }

    }
}
