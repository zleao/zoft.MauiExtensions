using zoft.MauiExtensions.Core.Commands;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Core.ViewModels;
using zoft.MauiExtensions.Sample.Localization;
using zoft.NotificationService;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class MainThreadViewModel : CoreViewModel
    {
        public ILocalizationService LocalizationService { get; }
        public INotificationService NotificationService { get; }

        public AsyncCommand ExecuteCommand { get; }

        public MainThreadViewModel(IMainThreadService mainThreadService, ILocalizationService localizationService, INotificationService notificationService) 
            : base(mainThreadService)
        {
            LocalizationService = localizationService;
            NotificationService = notificationService;

            ExecuteCommand = new AsyncCommand(OnExecuteAsync);
        }

        private async Task OnExecuteAsync()
        {
            await DoWorkAsync(async () => {
                await Task.Delay(2000).ConfigureAwait(false);

                await MainThreadService.InvokeOnMainThreadAsync(async () => 
                {
                    await NotificationService.PublishInfoNotificationAsync(LocalizationService[nameof(AppResources.MainThreadPage_Message)]);
                });
            },
            LocalizationService[nameof(AppResources.MainThreadPage_BusyMessage)]);
        }
    }
}
