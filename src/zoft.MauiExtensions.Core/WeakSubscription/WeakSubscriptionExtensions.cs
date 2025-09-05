using System.Collections.Specialized;
using System.ComponentModel;
using Zoft.MauiExtensions.Core.WeakSubscription;

namespace zoft.MauiExtensions.Core.WeakSubscription;

/// <summary>
/// Extensions to faciliate the usage of the WeakEventSubscription
/// </summary>
public static class WeakSubscriptionExtensions
{
    /// <summary>
    /// Subscribes to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event using a weak reference, ensuring
    /// the subscription does not prevent the source or target from being garbage collected.
    /// </summary>
    /// <remarks>This method is useful for scenarios where you want to avoid memory leaks caused by event
    /// subscriptions that prevent objects from being garbage collected. The subscription is automatically removed when
    /// either the source or target is collected by the garbage collector.</remarks>
    /// <param name="source">The object implementing <see cref="INotifyPropertyChanged"/> that raises the <see
    /// cref="INotifyPropertyChanged.PropertyChanged"/> event.</param>
    /// <param name="target">The target object that will handle the event. This is typically the object that owns the <paramref
    /// name="eventHandler"/>.</param>
    /// <param name="eventHandler">A callback that is invoked when the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised. The
    /// callback receives the source object, the target object, and the event arguments.</param>
    /// <returns>A <see cref="WeakEventSubscription{TSource, TTarget, TEventHandler, TEventArgs}"/> instance that manages the
    /// weak subscription. The caller should retain this object to keep the subscription active.</returns>
    public static WeakEventSubscription<INotifyPropertyChanged, TTarget, PropertyChangedEventHandler, PropertyChangedEventArgs> WeakSubscribe<TTarget>(
        this INotifyPropertyChanged source,
        TTarget target,
        Action<INotifyPropertyChanged, TTarget, PropertyChangedEventArgs> eventHandler) where TTarget : class
    {
        return new WeakEventSubscription<INotifyPropertyChanged, TTarget, PropertyChangedEventHandler, PropertyChangedEventArgs>(
            source,
            target,
            (p, h) => p.PropertyChanged += h,
            (p, h) => p.PropertyChanged -= h,
            cb => new PropertyChangedEventHandler(cb),
            eventHandler);
    }

    /// <summary>
    /// Subscribes to the <see cref="INotifyCollectionChanged.CollectionChanged"/> event using a weak reference,
    /// preventing memory leaks caused by strong event handler references.
    /// </summary>
    /// <remarks>This method ensures that the subscription to the <see
    /// cref="INotifyCollectionChanged.CollectionChanged"/> event does not prevent the garbage collection of the target
    /// object. Use this method to avoid memory leaks in scenarios where the event source has a longer lifetime than the
    /// subscriber.</remarks>
    /// <param name="source">The source object implementing <see cref="INotifyCollectionChanged"/> to subscribe to.</param>
    /// <param name="target">The target object that will handle the event. This is typically the subscriber.</param>
    /// <param name="eventHandler">A callback method that handles the <see cref="INotifyCollectionChanged.CollectionChanged"/> event. The callback
    /// receives the source object, the target object, and the event arguments.</param>
    /// <returns>A <see cref="WeakEventSubscription{TSource, TTarget, TEventHandler, TEventArgs}"/> instance that manages the
    /// weak subscription.</returns>
    public static WeakEventSubscription<INotifyCollectionChanged, TTarget, NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs> WeakSubscribe<TTarget>(
        this INotifyCollectionChanged source,
        TTarget target,
        Action<INotifyCollectionChanged, object?, NotifyCollectionChangedEventArgs> eventHandler) where TTarget : class
    {
        return new WeakEventSubscription<INotifyCollectionChanged, TTarget, NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
            source,
            target,
            (p, h) => p.CollectionChanged += h,
            (p, h) => p.CollectionChanged -= h,
            cb => new NotifyCollectionChangedEventHandler(cb),
            eventHandler);
    }

    /// <summary>
    /// Creates a weak subscription for an event, allowing the target to be garbage collected while the event
    /// subscription remains active.
    /// </summary>
    /// <remarks>This method is useful for scenarios where the event target should not prevent garbage
    /// collection of the source or itself. The weak subscription ensures that the event handler is automatically
    /// detached when the target is collected, preventing memory leaks.</remarks>
    /// <typeparam name="TSource">The type of the event source.</typeparam>
    /// <typeparam name="TTarget">The type of the event target.</typeparam>
    /// <typeparam name="TDelegate">The type of the delegate used for the event.</typeparam>
    /// <typeparam name="TArgs">The type of the event arguments.</typeparam>
    /// <param name="source">The source object that raises the event. Must not be <see langword="null"/>.</param>
    /// <param name="target">The target object that handles the event. Must not be <see langword="null"/>.</param>
    /// <param name="add">A delegate that adds the event handler to the source.</param>
    /// <param name="remove">A delegate that removes the event handler from the source.</param>
    /// <param name="delegateCreator">A function that creates the delegate to handle the event.</param>
    /// <param name="eventHandler">The event handler logic to execute when the event is raised.</param>
    /// <returns>A <see cref="WeakEventSubscription{TSource, TTarget, TDelegate, TArgs}"/> instance that manages the weak
    /// subscription.</returns>
    public static WeakEventSubscription<TSource, TTarget, TDelegate, TArgs> WeakSubscribe<TSource, TTarget, TDelegate, TArgs>(
        this TSource source,
        TTarget target,
        Action<TSource, TDelegate> add,
        Action<TSource, TDelegate> remove,
        Func<Action<object?, TArgs>, TDelegate> delegateCreator,
        Action<TSource, TTarget, TArgs> eventHandler) 
        where TSource : class 
        where TTarget : class
        where TDelegate : Delegate
    {
        return new WeakEventSubscription<TSource, TTarget, TDelegate, TArgs>(
            source, 
            target,
            add,
            remove,
            delegateCreator,
            eventHandler);
    }
}
