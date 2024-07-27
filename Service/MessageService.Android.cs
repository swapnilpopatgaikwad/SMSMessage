using Android.App;
using Android.Content;
using Android.Telephony;
using Application = Android.App.Application;

namespace SMSMessage.Service
{
    public partial class MessageService
    {
        TaskCompletionSource<(string, bool)> completionSource;
        public async Task<bool> SendMessageAsync(string phoneNumber, string message)
        {
            if (await RequestSmsPermissionAsync())
            {
                Intent deliveredIntent = new Intent(SmsSentReceiver.SmsDeliveredAction);
                deliveredIntent.PutExtra("PHONE_NUMBER", phoneNumber);
                var sentIntent = PendingIntent.GetBroadcast(Application.Context, 0, deliveredIntent, PendingIntentFlags.Immutable);
                SmsSentReceiver.SmsSent += OnSmsSent;

                SmsManager smsManager = SmsManager.Default;
                smsManager.SendTextMessage(phoneNumber, null, message, sentIntent, null);

                completionSource = new TaskCompletionSource<(string, bool)>();

                return (await completionSource.Task).Item2;
            }
            else
            {
                Console.WriteLine("SMS permission is required to send messages.");
                return false;
            }
        }

        private void OnSmsSent(object? sender, SmsSentEventArgs e)
        {
            SmsSentReceiver.SmsSent -= OnSmsSent;
            completionSource.SetResult((e.PhoneNumber,e.IsSuccess));
        }

        public async Task<List<(string, bool)>> SendMessageAsync(List<string> phoneNumbers, string message)
        {
            var resultList = new List<(string, bool)>();
            if (await RequestSmsPermissionAsync())
            {
                Intent deliveredIntent = new Intent(SmsSentReceiver.SmsDeliveredAction);
                var sentIntent = PendingIntent.GetBroadcast(Application.Context, 0, deliveredIntent, PendingIntentFlags.Immutable);
                

                SmsManager smsManager = SmsManager.Default;
                foreach (var phoneNumber in phoneNumbers)
                {
                    SmsSentReceiver.SmsSent += OnSmsSent;
                    completionSource = new TaskCompletionSource<(string, bool)>();
                    smsManager.SendTextMessage(phoneNumber, null, message, sentIntent, null);
                    var status= await completionSource.Task;
                    resultList.Add(status);
                }
                return resultList;
            }
            else
            {
                Console.WriteLine("SMS permission is required to send messages.");
                phoneNumbers.ForEach(x => resultList.Add((x, false)));
                return resultList;
            }
        }
    }
}
