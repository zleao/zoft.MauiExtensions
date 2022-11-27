using System.Collections.Specialized;
using System.Reflection;

namespace zoft.MauiExtensions.Core.WeakSubscription
{
    /// <summary>
    /// Weak subscrition for CollectionChanged events
    /// </summary>
    public class NotifyCollectionChangedEventSubscription
        : WeakEventSubscription<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>
    {
        private static readonly EventInfo EventInfo = typeof(INotifyCollectionChanged).GetEvent("CollectionChanged");

        /// <summary>
        /// This code ensures the CollectionChanged event is not stripped by Xamarin linker
        /// </summary>
        /// <param name="iNotifyCollectionChanged">The i notify collection changed.</param>
        public static void LinkerPleaseInclude(INotifyCollectionChanged iNotifyCollectionChanged)
        {
            iNotifyCollectionChanged.CollectionChanged += (sender, e) => { };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyCollectionChangedEventSubscription"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetEventHandler">The target event handler.</param>
        public NotifyCollectionChangedEventSubscription(INotifyCollectionChanged source,
                                                        EventHandler<NotifyCollectionChangedEventArgs> targetEventHandler)
            : base(source, EventInfo, targetEventHandler)
        {
        }

        /// <summary>
        /// Creates the event handler.
        /// </summary>
        /// <returns></returns>
        protected override Delegate CreateEventHandler()
        {
            return new NotifyCollectionChangedEventHandler(OnSourceEvent);
        }
    }
}