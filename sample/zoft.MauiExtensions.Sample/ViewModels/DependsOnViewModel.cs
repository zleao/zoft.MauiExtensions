﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using zoft.MauiExtensions.Core.Models;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class DependsOnViewModel : ZoftObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TargetText))]
        private string _triggerText;
        partial void OnTriggerTextChanged(string value)
        {
            TargetDate = TargetDate.AddDays(1);
        }

        public string TargetText => $"TargetText triggered '{TriggerText}'";

        [ObservableProperty]
        private DateTime _targetDate = DateTime.Now;

        public DependsOnViewModel()
            : base()
        {
        }

        [RelayCommand]
        private async Task OpenLink(string url)
        {
            await Browser.Default.OpenAsync(url);
        }
    }
}
