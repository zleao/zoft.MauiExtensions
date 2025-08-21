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



### Base Models

Based on the Component Models of the [CommunityToolkit.MVVM](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/), the package provides a set of base models that can be used to create Models, ViewModels and Services in your app.

- **ZoftObservableObject**: Based on the `ObservableObject` class, provides a base implementation of the `INotifyPropertyChanged` interface with additional features:
  - `IsBusy` and `IsNotBusy` properties for UI binding
  - `BusyMessage` property for displaying status messages
  - `DoWorkAsync()` methods for executing background tasks with busy state management
  - Implements `IDisposable` for proper resource cleanup

- **ZoftObservableRecipient**: Based on the `ObservableRecipient` class, provides messaging capabilities:
  - Inherits all features from `ZoftObservableObject`
  - Built-in messenger functionality for communication between ViewModels
  - Automatic message registration and cleanup
  - `Broadcast()` method for sending property change messages

- **ZoftObservableValidator**: Based on the `ObservableValidator` class, provides validation capabilities:
  - All features of `ZoftObservableObject`
  - Data annotation validation support
  - `ValidateAllProperties()` and `ClearErrors()` methods
  - Override `OnErrorsChanged()` to handle validation state changes
  - Automatic error collection and management

</br>

### Busy Notification Management
Base models provide methods to execute code in a background thread, while providing with updated on `IsBusy` and `BusyMessage` properties that can be bound to an UI element (i.e. `ActivityIndicator`)
```csharp
await DoWorkAsync(() => ..., "Busy Message");

var result = await DoWorkAsync(() => return some_object, "BusyMessage");
```

</br>

### Validation

The `ZoftObservableValidator` base class provides comprehensive validation capabilities using data annotations:

```csharp
public partial class ValidationViewModel : ZoftObservableValidator
{
    [ObservableProperty]
    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public partial string FirstName { get; set; }

    [ObservableProperty]
    [Required]
    [EmailAddress]
    public partial string Email { get; set; }

    protected override void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        base.OnErrorsChanged(sender, e);
        
        // Handle validation state changes
        ErrorMessage = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
    }

    [RelayCommand]
    private void Validate()
    {
        ValidateAllProperties();
    }

    [RelayCommand]
    private void ClearValidation()
    {
        ClearErrors();
    }
}
```

</br>

### Messenger and Communication

The `ZoftObservableRecipient` base class provides built-in messaging capabilities for ViewModel communication:

```csharp
public partial class MessengerViewModel : ZoftObservableRecipient, 
    IRecipient<PropertyChangedMessage<string>>, 
    IRecipient<CustomMessage>
{
    public MessengerViewModel() : base()
    {
        IsActive = true; // Enable message reception
    }

    // Receive property change messages
    void IRecipient<PropertyChangedMessage<string>>.Receive(PropertyChangedMessage<string> message)
    {
        if (message.PropertyName == nameof(Text))
            Text = message.NewValue;
    }

    // Receive custom messages
    void IRecipient<CustomMessage>.Receive(CustomMessage message)
    {
        // Handle custom message
    }

    [RelayCommand]
    private void SendMessage()
    {
        // Broadcast property changes
        Broadcast(oldValue, newValue, nameof(PropertyName));
        
        // Send custom messages
        Messenger.Send<CustomMessage>();
    }
}
```

</br>

### Weak Subscription

The package provides a set of extension methods to subscribe to events using weak references, avoiding memory leaks when the subscriber is not disposed properly:

```csharp
// Subscribe to PropertyChanged events
INotifyPropertyChanged source = someObject;
var subscription = source.WeakSubscribe((sender, e) => 
{
    // Handle property change
    Console.WriteLine($"Property {e.PropertyName} changed");
});

// Subscribe to CollectionChanged events
INotifyCollectionChanged collection = observableCollection;
var collectionSubscription = collection.WeakSubscribe((sender, e) => 
{
    // Handle collection change
    Console.WriteLine($"Collection changed: {e.Action}");
});

// Subscribe to generic events
var eventSubscription = someObject.WeakSubscribe("SomeEvent", (sender, e) => 
{
    // Handle generic event
});

// Subscribe to events with custom event args
var customSubscription = someObject.WeakSubscribe<CustomEventArgs>("CustomEvent", (sender, e) => 
{
    // Handle custom event
});

// Dispose subscriptions when no longer needed
subscription.Dispose();
collectionSubscription.Dispose();
eventSubscription.Dispose();
customSubscription.Dispose();
```

</br>

### Extensions

The library provides numerous extension methods to simplify common operations:

#### Collection Extensions
```csharp
// Add missing items to a collection
targetCollection.AddMissing(itemsToAdd);
targetCollection.AddMissing(itemsToAdd, customValidationFunc);

// Safe count with fallback
int count = collection.Count(fallbackValue: 0);
```

#### String Extensions
```csharp
// Null-safe string operations
bool isEmpty = text.IsNullOrEmpty();
bool isWhitespace = text.IsNullOrWhiteSpace();

// Template formatting with null safety
string result = template.FormatTemplate(arg1, arg2);

// Regular expression matching
bool matches = input.IsMatch(@"\d+");
string extract = input.Extract(@"(\d+)");
```

#### DateTime Extensions
```csharp
// Null-safe formatting
string formatted = nullableDate.Format("yyyy-MM-dd");

// Date operations
DateTime dateOnly = dateTime.ToDate();
string shortDate = dateTime.ToShortDateString();
DateTime utcAdjusted = dateTime.GetUtcAdjustedTime();
```

#### Object Extensions
```csharp
// Reflection-based property access
object value = obj.GetPropertyValue("PropertyName");
string stringValue = obj.GetPropertyValueAsString("PropertyName", "default");
obj.SetPropertyValue("PropertyName", newValue);

// Null checking
var nonNull = obj.ThrowIfNull(nameof(obj));
```

#### Task Extensions
```csharp
// Timeout support
var result = await task.WithTimeout(5000); // 5 seconds
var result = await task.WithTimeout(TimeSpan.FromMinutes(1));

// Safe fire-and-forget execution
task.SafeFireAndForget(onException: ex => Console.WriteLine(ex.Message));
```

#### List Extensions
```csharp
// Find index with predicate
int index = list.FindIndex(item => item.Name == "test");

// Apply action to all items
list.ForEach(item => item.Process());
```

#### Dictionary Extensions
```csharp
// Safe operations
dictionary.AddOrUpdate(key, value);
dictionary.AddOrUpdate(keyValuePairs); // Bulk operations
dictionary.AddOrIgnore(keyValuePairs); // Add only if key doesn't exist
dictionary.RemoveIfExists(key);

// Apply action to all items
dictionary.ForEach((key, value) => Console.WriteLine($"{key}: {value}"));

// Convert to tuple list
var tuples = dictionary.ToTupleList();
```

#### Exception Extensions
```csharp
// Get full exception description including inner exceptions
string fullDescription = exception.GetFullDescription();
```

#### Type Extensions
```csharp
// Get properties with various options
var publicProps = type.GetProperties(onlyPublic: true);
var allProps = type.GetProperties(onlyPublic: false, includeInherited: true);
```
