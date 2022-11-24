using zoft.MauiExtensions.Sample.ViewModels;

namespace zoft.MauiExtensions.Sample.Views
{
    public partial class LocalizationView : ContentPage
    {
        public LocalizationView(LocalizationViewModel bindingContext)
        {
            BindingContext = bindingContext;

            InitializeComponent();
        }
    }
}