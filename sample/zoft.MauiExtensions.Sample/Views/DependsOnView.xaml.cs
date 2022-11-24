using zoft.MauiExtensions.Sample.ViewModels;

namespace zoft.MauiExtensions.Sample.Views
{
    public partial class DependsOnView : ContentPage
    {
        public DependsOnView(DependsOnViewModel bindingContext)
        {
            BindingContext = bindingContext;

            InitializeComponent();
        }
    }
}