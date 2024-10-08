# zoft.MauiExtensions

Set of tools designed to be used in MAUI projects, including Views, ViewModels, Services, Extensions and more...

Nuget Package | Current Version
--- | ---
| zoft.MauiExtensions.Core | [![NuGet](https://img.shields.io/nuget/v/zoft.MauiExtensions.Core.svg)](https://www.nuget.org/packages/zoft.MauiExtensions.Core/)


## Getting Started

### Install nuget package: [zoft.MauiExtensions.Core](https://www.nuget.org/packages/zoft.MauiExtensions.Core/)
 
```
Install-Package zoft.MauiExtensions.Core
```

### Create Windows `MainThreadService`

```csharp
using Microsoft.UI.Dispatching;
using zoft.MauiExtensions.Core.Services;

namespace <your.app.base.namespace>.Platforms.Windows.Services
{
    public sealed class WindowsMainThreadService : MainThreadService
    {
        protected override DispatcherQueue MainThreadDispatcher => WinUI.App.Dispatcher;
    }
}
```

### Register `MainThreadService` depending on platform

```csharp
return builder
    .UseMauiApp<App>()
    .ConfigureFonts(fonts =>
    {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
    })
#if WINDOWS
    .Services.AddSingleton<IMainThreadService, Platforms.Windows.Services.WindowsMainThreadService>();
#else
    .Services.AddSingleton<IMainThreadService, MainThreadService>();
#endif
    .Build();
```

## How To Use
Refer to the sample to have a better understanding of package capabilities. Bellow you can find the most common features and how to use them

</br>

### Localization Service

The package provides a set of tools to implement localization in your app:
- `ILocalizationService`: Interface for the localization service. The interface exists to make it easier to use iwith IOC and to override the base implementation
- `ResourceManagerLocalizationService`: Implementation of the `ILocalizationService` using resource files (*.resx*)
```csharp
builder.Services.AddSingleton<ILocalizationService>(new ResourceManagerLocalizationService(AppResources.ResourceManager, SupportedLanguages.DefaultLanguage));
```
- `TranslationMarkup`: XAML markup that provides an easy way to apply the translation directly in XAML
```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:zoft="http://zoft.maui.extensions"
             x:Class="zoft.MauiExtensions.Sample.Views.LocalizationView"
             Title="{zoft:Translate LocalizationPage_Title}">
    ...
    ...
    <ScrollView>
        <VerticalStackLayout Spacing="10">
            <Label Text="{zoft:Translate LocalizationPage_Label1}" FontSize="16" FontAttributes="Bold"/>
            <Label Text="{zoft:Translate LocalizationPage_Label2}" />
            <Label Text="{zoft:Translate LocalizationPage_Label3}" FontAttributes="Italic" BackgroundColor="LightGray"/>
        </VerticalStackLayout>
    </ScrollView>
    ...
</ContentPage>
```

</br>

### MainThread Service

At current time, the MAUI essentials implementation of `MainThread` is not working for Windows. This package provides a wrapper interface `IMainThreadService` and an implementation for all platforms, including Windows.
To use, you just need to register the correct service depending of the platform:
```csharp
public sealed class WindowsMainThreadService : MainThreadService
{
    protected override DispatcherQueue MainThreadDispatcher => WinUI.App.Dispatcher;
}
```

</br>

### Base Models

Based on the Component Models of the [CommunityToolkit.MVVM](), the package provides a set of base models that can be used to create Models, ViewModels and Services in your app.

- **ZoftObservableObject**: Based on the `ObservableObject` class, provides a base implementation of the `INotifyPropertyChanged` interface
- **ZoftObservableRecipient**: Based on the `ObservableRecipient` class, provides a base implementation of the `ObservableRecipient` class
- **ZoftObservableValidator**: Based on the `ObservableValidator` class, provides a base implementation of the `ObservableValidator` class
    - Override `OnErrorsChanged` to handle logic when the validation results change

</br>

### Busy Notification Management
Base models provide methods to execute code in a background thread, while providing with updated on `IsBusy` and `BusyMessage` properties that can be bound to an UI element (i.e. `ActivityIndicator`)
```csharp
await DoWorkAsync(() => ..., "Busy Message");

var result = await DoWorkAsync(() => return some_object, "BusyMessage");
```

</br>

### Weak Subscription

The package provides a set of extension methods to subscribe to events using weak references, avoiding memory leaks when the subscriber is not disposed.
