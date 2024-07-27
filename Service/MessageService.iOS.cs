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

        public async Task<List<(string, bool)>> SendMessageAsync(List<string> phoneNumbers, string message)
        {
            var resultList = new List<(string, bool)>();

            foreach (var phoneNumber in phoneNumbers)
            {
                var url = new NSUrl($"sms:{phoneNumber}&body={message}");
                var status = UIApplication.SharedApplication.OpenUrl(url);
                resultList.Add((phoneNumber,status));
            }

            return resultList;
        }
    }
}
