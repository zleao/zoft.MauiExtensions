using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace zoft.MauiExtensions.Core.WeakSubscription
{
    /// <summary>
    /// Extensions to faciliate the usage of the WeakEventSubscription
    /// </summary>
    public static class WeakSubscriptionExtensions
    {
        /// <summary>
        /// Creates a weak subscription for a PropertyChanged event
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <returns></returns>
        public static NotifyPropertyChangedEventSubscription WeakSubscribe(this INotifyPropertyChanged source,
                                                                           EventHandler<PropertyChangedEventArgs> eventHandler)
        {
            return new NotifyPropertyChangedEventSubscription(source, eventHandler);
        }

        /// <summary>
        /// Creates a weak subscription for a CollectionChanged event
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <returns></returns>
        public static NotifyCollectionChangedEventSubscription WeakSubscribe(this INotifyCollectionChanged source,
                                                                             EventHandler<NotifyCollectionChangedEventArgs> eventHandler)
        {
            return new NotifyCollectionChangedEventSubscription(source, eventHandler);
        }

        /// <summary>
        /// Creates a weak subscription for a generic event
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <returns></returns>
        public static WeakEventSubscription<TSource> WeakSubscribe<TSource>(this TSource source, string eventName, EventHandler eventHandler)
            where TSource : class
        {
            return new WeakEventSubscription<TSource>(source, eventName, eventHandler);
        }

        /// <summary>
        /// Creates a weak subscription for a generic event wit custom event args
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="eventHandler">The event handler.</param>
        /// <returns></returns>
        public static WeakEventSubscription<TSource, TEventArgs> WeakSubscribe<TSource, TEventArgs>(this TSource source, string eventName, EventHandler<TEventArgs> eventHandler)
            where TSource : class
        {
            return new WeakEventSubscription<TSource, TEventArgs>(source, eventName, eventHandler);
        }
    }
}
