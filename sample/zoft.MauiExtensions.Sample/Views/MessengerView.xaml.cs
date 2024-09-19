using zoft.MauiExtensions.Sample.ViewModels;

namespace zoft.MauiExtensions.Sample.Views
{
    public partial class MessengerView : ContentPage
    {
        public MessengerView(MessengerViewModel bindingContext)
        {
            BindingContext = bindingContext;

            InitializeComponent();
        }
    }
}