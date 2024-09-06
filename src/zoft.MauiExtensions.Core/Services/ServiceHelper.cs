namespace zoft.MauiExtensions.Core.Services;

/// <summary>
/// Service Helper to expose a generic access to MAUI ServiceProvider
/// </summary>
public static class ServiceHelper
{
    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the MAUI service provider.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns></returns>

    public static T GetService<T>() => Current.GetService<T>();

    /// <summary>
    /// Current ServiceProvider
    /// </summary>
    public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
        MauiWinUIApplication.Current.Services;
#elif ANDROID || IOS || MACCATALYST
        IPlatformApplication.Current.Services;
#else
        throw new NotSupportedException("ServiceProvider not supported in current platform");
#endif 
}
