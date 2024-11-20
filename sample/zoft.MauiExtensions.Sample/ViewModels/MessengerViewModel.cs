using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using zoft.MauiExtensions.Core.Models;
using zoft.MauiExtensions.Core.Services;

namespace zoft.MauiExtensions.Sample.ViewModels
{
    public partial class MessengerViewModel : ZoftObservableRecipient, IRecipient<PropertyChangedMessage<string>>, IRecipient<ClearTextMessage>
    {
        [ObservableProperty]
        private string _text = "";

        public MessengerViewModel() : base()
        {
            IsActive = true;
        }

        void IRecipient<PropertyChangedMessage<string>>.Receive(PropertyChangedMessage<string> message)
        {
            if (message.PropertyName == nameof(Text))
                Text = message.NewValue;
        }

        void IRecipient<ClearTextMessage>.Receive(ClearTextMessage message)
        {
            Text = "";
        }

        [RelayCommand]
        private void SendMessage()
        {
            Broadcast(Text, Text + "i", nameof(Text));
        }

        [RelayCommand]
        private void ClearText()
        {
            Messenger.Send<ClearTextMessage>();
        }
    }

    internal class ClearTextMessage
    {
    }
}
