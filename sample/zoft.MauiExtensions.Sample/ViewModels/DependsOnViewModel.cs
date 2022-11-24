using CommunityToolkit.Mvvm.ComponentModel;
using zoft.MauiExtensions.Core.Attributes;
using zoft.MauiExtensions.Core.ViewModels;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class DependsOnViewModel : CoreViewModel
    {
        [ObservableProperty]
        private string _triggerText;

        [DependsOn(nameof(TriggerText))]
        public string TargetText => $"TargetText triggered '{TriggerText}'";

        [ObservableProperty]
        private DateTime _targetDate = DateTime.Now;

        [DependsOn(nameof(TriggerText))]
        protected void OnTriggerTextChanged()
        {
            TargetDate = TargetDate.AddDays(1);
        }

    }
}
