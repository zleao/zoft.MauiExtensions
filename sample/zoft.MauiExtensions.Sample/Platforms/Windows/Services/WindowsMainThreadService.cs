using Microsoft.UI.Dispatching;
using zoft.MauiExtensions.Core.Services;

namespace zoft.MauiExtensions.Sample.Platforms.Windows.Services
{
    public sealed class WindowsMainThreadService : MainThreadService
    {
        protected override DispatcherQueue MainThreadDispatcher => WinUI.App.Dispatcher;
    }
}
