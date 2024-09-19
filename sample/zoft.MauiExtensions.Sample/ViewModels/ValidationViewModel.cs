using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using zoft.MauiExtensions.Core.Models;
using zoft.MauiExtensions.Core.Services;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class ValidationViewModel : ZoftObservableValidator
    {
        [ObservableProperty]
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        private string _firstName;

        [ObservableProperty]
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        private string _lastName;

        [ObservableProperty]
        [Required]
        [EmailAddress]
        private string _email;

        [ObservableProperty]
        [Required]
        [Phone]
        private string _phoneNumber;

        [ObservableProperty]
        private string _errorMessage;

        public ValidationViewModel(IMainThreadService mainThreadService) : base(mainThreadService)
        {

        }

        protected override void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            base.OnErrorsChanged(sender, e);

            ErrorMessage = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
        }

        [RelayCommand]
        private void Validate()
        {
            ValidateAllProperties();
        }

        [RelayCommand]
        private void ClearValidation()
        {
            ClearErrors();
        }

        [RelayCommand]
        private async Task OpenLink(string url)
        {
            await Browser.Default.OpenAsync(url);
        }
    }
}
