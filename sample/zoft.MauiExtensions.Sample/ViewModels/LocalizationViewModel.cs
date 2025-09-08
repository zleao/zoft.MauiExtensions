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

        public List<string> Languages => SupportedLanguages.List.Select(l => l.Name).ToList();

        [ObservableProperty]
        public partial string SelectedLanguage { get; set; }
        partial void OnSelectedLanguageChanged(string value)
        {
            LocalizationService.SetLanguage(SupportedLanguages.List.First(l => l.Name == value).Info);
        }

        public LocalizationViewModel(ILocalizationService localizationService)
            : base()
        {
            LocalizationService = localizationService;

            SelectedLanguage = SupportedLanguages.List.FirstOrDefault(l => LocalizationService.CurrentCulture == l.Info).Name;
        }
    }
}
