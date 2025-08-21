using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using zoft.MauiExtensions.Core.Models;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Sample.Localization;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class LocalizationViewModel : ZoftObservableObject
    {
        public ILocalizationService LocalizationService { get; }

        public List<CultureInfo> Languages => SupportedLanguages.List;

        [ObservableProperty]
        public partial CultureInfo SelectedLanguage { get; set; }
        partial void OnSelectedLanguageChanged(CultureInfo value)
        {
            LocalizationService.SetLanguage(value);
        }

        public LocalizationViewModel(ILocalizationService localizationService)
            : base()
        {
            LocalizationService = localizationService;

            SelectedLanguage = LocalizationService.CurrentCulture;
        }
    }
}
