namespace zoft.MauiExtensions.Core.Services
{
    public static class ServiceHelper
    {

        public static T GetService<T>() => Current.GetService<T>();

        public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
            MauiWinUIApplication.Current.Services;
#elif ANDROID
            MauiApplication.Current.Services;
#elif IOS || MACCATALYST
            MauiUIApplicationDelegate.Current.Services;
#else
            throw new NotSupportedException("ServiceProvider not supported in current platform");
#endif 
    }
}
