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
        public partial string FirstName { get; set; }

        [ObservableProperty]
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public partial string LastName { get; set; }

        [ObservableProperty]
        [Required]
        [EmailAddress]
        public partial string Email { get; set; }

        [ObservableProperty]
        [Required]
        [Phone]
        public partial string PhoneNumber { get; set; }

        [ObservableProperty]
        public partial string ErrorMessage { get; set; }

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
