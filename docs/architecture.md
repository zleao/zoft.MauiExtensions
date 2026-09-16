# Architecture and behavioral constraints

## Library and sample

The [core project](../src/zoft.MauiExtensions.Core/zoft.MauiExtensions.Core.csproj) is the published package. It targets plain .NET plus MAUI platforms, with Windows added on Windows hosts. The [sample](../sample/zoft.MauiExtensions.Sample/zoft.MauiExtensions.Sample.csproj) references it directly.

`Models` contains three separate CommunityToolkit-derived base classes: `ZoftObservableObject`, `ZoftObservableRecipient`, and `ZoftObservableValidator`. Busy-state and disposal behavior is repeated across these classes; check all three when changing shared semantics. They are not a single inheritance chain.

`Services` and `Markup` implement localization; `WeakSubscription` handles event lifetime; `Extensions` contains general-purpose helpers. Public namespaces are not uniformly cased (for example, weak subscriptions use `Zoft`); preserve existing names for consumers.

## Busy state and threading

[ZoftObservableObject](../src/zoft.MauiExtensions.Core/Models/ZoftObservableObject.cs) illustrates the busy counter, inverse `IsBusy`/`IsNotBusy` notifications, and message cleanup. Preserve balanced start/finish behavior for successful and failed work, silent operations, and overlapping work. Do not replace the counter with a single boolean without considering overlapping operations.

`DoWorkAsync` invokes the supplied delegate and awaits it with `ConfigureAwait(false)`. It does not automatically run the delegate on a background thread or marshal notifications to the UI thread. Avoid documenting or relying on either guarantee. UI work should explicitly dispatch where required, as in [MainThreadViewModel](../sample/zoft.MauiExtensions.Sample/ViewModels/MainThreadViewModel.cs). An interlocked counter alone does not establish thread safety for all model state.

## Ownership and weak subscriptions

[WeakEventSubscription](../src/zoft.MauiExtensions.Core/WeakSubscription/WeakEventSubscription.cs) retains the source and delegates, but holds the target through a weak reference. Use static callbacks where possible: capturing the target in a retained delegate defeats the weak-reference design. The subscription detaches on disposal and can detach when an event discovers that the target has been collected; collection alone is not an immediate cleanup callback.

Keep the subscription token with its owner and dispose it when that owner is disposed. Follow [ZoftObservableValidator](../src/zoft.MauiExtensions.Core/Models/ZoftObservableValidator.cs) for the validation subscription and `OnErrorsChanged` hook. Preserve base cleanup when overriding disposal hooks and avoid duplicate event registration. Do not infer lifetime correctness from navigation checks alone.

## Localization and sample registration

[MauiProgram](../sample/zoft.MauiExtensions.Sample/MauiProgram.cs) registers `ILocalizationService`, views, and viewmodels. Sample feature views and viewmodels use singleton registrations; navigation does not necessarily construct fresh instances.

[Translate](../src/zoft.MauiExtensions.Core/Markup/TranslationMarkup.cs) creates a one-way indexer binding whose source is resolved through `ServiceHelper`. Register the localization service before translated views are created. [ResourceManagerLocalizationService](../src/zoft.MauiExtensions.Core/Services/Implementations/ResourceManagerLocalizationService.cs) changes its selected culture and raises property-change notifications to refresh bindings; preserve that refresh behavior.

Add localized text through the sample's `Localization/AppResources*.resx` inputs and keep supported translations aligned. Use `{zoft:Translate ...}` in XAML and `ILocalizationService` in viewmodels. Preserve `x:DataType`, constructor injection, and Shell navigation wiring when adding feature pages.

## Public API changes

Existing consumers depend on public names, signatures, generic constraints, exception behavior, and notification semantics. Treat changes to these as compatibility decisions. Add XML documentation for new public APIs, update the sample where useful, and describe consumer-visible changes in the changelog. Use [versioning.md](versioning.md) when deciding how a release represents a breaking change.
