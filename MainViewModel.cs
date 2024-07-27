using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SMSMessage.Interface;
using SMSMessage.Service;

namespace SMSMessage
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string mobileNumber;

        [ObservableProperty]
        private string message;
        private readonly IMessageService messageService;
        public MainViewModel()
        {
            messageService = AppServiceProvider.GetService<IMessageService>();
        }

        [RelayCommand]
        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(MobileNumber) || string.IsNullOrWhiteSpace(Message))
            {
                await ShowAlert("Error", "Please enter both mobile number and message.");
                return;
            }

            var status = await messageService.SendMessageAsync(MobileNumber, Message);

            if (status)
            {
                await ShowAlert("Success", "Message sent successfully!");
                ClearFields();
            }
            else
            {
                await ShowAlert("Failure", "Failed to send the message.");
            }
        }

        private async Task ShowAlert(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        private void ClearFields()
        {
            MobileNumber = string.Empty;
            Message = string.Empty;
        }
    }
}
