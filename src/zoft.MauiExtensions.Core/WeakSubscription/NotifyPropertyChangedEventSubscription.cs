using System;
using System.ComponentModel;
using System.Reflection;

namespace zoft.MauiExtensions.Core.WeakSubscription
{
    /// <summary>
    /// Weak subscrition for PropertyChanged events
    /// </summary>
    public class NotifyPropertyChangedEventSubscription
        : WeakEventSubscription<INotifyPropertyChanged, PropertyChangedEventArgs>
    {
        private static readonly EventInfo PropertyChangedEventInfo = typeof(INotifyPropertyChanged).GetEvent("PropertyChanged");

        /// <summary>
        /// This code ensures the PropertyChanged event is not stripped by Xamarin linker
        /// </summary>
        /// <param name="iNotifyPropertyChanged">The i notify property changed.</param>
        public static void LinkerPleaseInclude(INotifyPropertyChanged iNotifyPropertyChanged)
        {
            iNotifyPropertyChanged.PropertyChanged += (sender, e) => { };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyPropertyChangedEventSubscription"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="targetEventHandler">The target event handler.</param>
        public NotifyPropertyChangedEventSubscription(INotifyPropertyChanged source,
                                                      EventHandler<PropertyChangedEventArgs> targetEventHandler)
            : base(source, PropertyChangedEventInfo, targetEventHandler)
        {
        }

        /// <summary>
        /// Creates the event handler.
        /// </summary>
        /// <returns></returns>
        protected override Delegate CreateEventHandler()
        {
            return new PropertyChangedEventHandler(OnSourceEvent);
        }
    }
}