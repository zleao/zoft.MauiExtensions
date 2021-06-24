using System.Collections.Specialized;
using zoft.MauiExtensions.Core.WeakSubscription;

namespace zoft.MauiExtensions.Core.Models
{
    internal class CollectionSubscriptionInfo
    {
        public INotifyCollectionChanged Collection { get; set; }
        public NotifyCollectionChangedEventSubscription CollectionChangedSubscription { get; set; }
    }
}
