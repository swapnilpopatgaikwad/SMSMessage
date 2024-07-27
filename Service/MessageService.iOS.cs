using Foundation;
using UIKit;

namespace SMSMessage.Service
{
    public partial class MessageService
    {
        public async Task<bool> SendMessageAsync(string phoneNumber, string message)
        {
            var url = new NSUrl($"sms:{phoneNumber}&body={message}");
            return UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}
