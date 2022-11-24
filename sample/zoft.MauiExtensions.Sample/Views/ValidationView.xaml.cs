using zoft.MauiExtensions.Sample.ViewModels;

namespace zoft.MauiExtensions.Sample.Views
{
    public partial class ValidationView : ContentPage
    {
        public ValidationView(ValidationViewModel bindingContext)
        {
            BindingContext = bindingContext;

            InitializeComponent();
        }
    }
}