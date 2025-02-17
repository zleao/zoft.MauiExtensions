using CommunityToolkit.Mvvm.Input;
using zoft.MauiExtensions.Core.Models;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Sample.Localization;
using zoft.NotificationService;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class MainThreadViewModel(ILocalizationService localizationService,
                                             INotificationService notificationService) 
        : ZoftObservableObject()
    {
        public ILocalizationService LocalizationService { get; } = localizationService;
        public INotificationService NotificationService { get; } = notificationService;

        [RelayCommand]
        private async Task Execute()
        {
            await DoWorkAsync(async () => {
                await Task.Delay(2000).ConfigureAwait(false);

                await MainThread.InvokeOnMainThreadAsync(async () => 
                {
                    await NotificationService.PublishInfoNotificationAsync(LocalizationService[nameof(AppResources.MainThreadPage_Message)]);
                });
            },
            LocalizationService[nameof(AppResources.MainThreadPage_BusyMessage)]);
        }
    }
}
