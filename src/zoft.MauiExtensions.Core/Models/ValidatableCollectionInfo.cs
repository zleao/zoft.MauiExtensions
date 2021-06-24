using zoft.MauiExtensions.Core.Validation;
using zoft.MauiExtensions.Core.WeakSubscription;

namespace zoft.MauiExtensions.Core.Models
{
    internal class ValidatableCollectionInfo
    {
        public IValidatable ValidatableObject { get; set; }
        public NotifyPropertyChangedEventSubscription PropertyChangedSubscription { get; set; }
    }
}
