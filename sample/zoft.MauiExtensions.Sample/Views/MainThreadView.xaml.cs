using zoft.MauiExtensions.Sample.ViewModels;
using zoft.NotificationService;
using zoft.NotificationService.Core;
using zoft.NotificationService.Messages.OneWay;

namespace zoft.MauiExtensions.Sample.Views
{
    public partial class MainThreadView : ContentPage
    {
        public INotificationService NotificationService { get; }

        SubscriptionToken _notificationToken;

        public MainThreadView(MainThreadViewModel bindingContext, INotificationService notificationService)
        {
            BindingContext = bindingContext;
            NotificationService = notificationService;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            NotificationService.Unsubscribe(_notificationToken);
            _notificationToken = NotificationService.Subscribe<NotificationGenericMessage>(OnNotificationGenericMessageAsync);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            NotificationService?.Unsubscribe(_notificationToken);
        }


        private async Task OnNotificationGenericMessageAsync(NotificationGenericMessage msg)
        {
            await DisplayAlert(msg.Severity.ToString(), msg.Message, "OK");
        }
    }
}