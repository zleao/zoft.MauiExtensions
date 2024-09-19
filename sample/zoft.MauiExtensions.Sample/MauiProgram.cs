using CommunityToolkit.Maui;
using zoft.MauiExtensions.Core.Services;
using zoft.MauiExtensions.Sample.Localization;
using zoft.MauiExtensions.Sample.ViewModels;
using zoft.MauiExtensions.Sample.Views;
using zoft.NotificationService;

namespace zoft.MauiExtensions.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .RegisterServices()
                .RegisterViewModels()
                .RegisterViews()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }

        private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<ILocalizationService>(new ResourceManagerLocalizationService(AppResources.ResourceManager, SupportedLanguages.DefaultLanguage));
            builder.Services.AddSingleton<INotificationService, NotificationManager>();

#if WINDOWS
            builder.Services.AddSingleton<IMainThreadService, Platforms.Windows.Services.WindowsMainThreadService>();
#else
            builder.Services.AddSingleton<IMainThreadService, MainThreadService>();
#endif
            return builder;
        }

        private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<DependsOnViewModel>();
            builder.Services.AddSingleton<LocalizationViewModel>();
            builder.Services.AddSingleton<MainThreadViewModel>();
            builder.Services.AddSingleton<MessengerViewModel>();
            builder.Services.AddSingleton<ValidationViewModel>();

            return builder;
        }

        private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<DependsOnView>();
            builder.Services.AddSingleton<LocalizationView>();
            builder.Services.AddSingleton<MainThreadView>();
            builder.Services.AddSingleton<MessengerView>();
            builder.Services.AddSingleton<ValidationView>();

            return builder;
        }
    }
}