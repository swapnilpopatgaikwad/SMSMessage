using Android.App;
using Android.Content;
using Android.Telephony;
using Application = Android.App.Application;

namespace SMSMessage.Service
{
    public partial class MessageService
    {
        TaskCompletionSource<bool> completionSource;
        public async Task<bool> SendMessageAsync(string phoneNumber, string message)
        {
            if (await RequestSmsPermissionAsync())
            {
                Intent deliveredIntent = new Intent(SmsSentReceiver.SmsDeliveredAction);
                var sentIntent = PendingIntent.GetBroadcast(Application.Context, 0, deliveredIntent, PendingIntentFlags.Immutable);
                SmsSentReceiver.SmsSent += OnSmsSent;

                SmsManager smsManager = SmsManager.Default;
                smsManager.SendTextMessage(phoneNumber, null, message, sentIntent, null);

                completionSource = new TaskCompletionSource<bool>();

                return await completionSource.Task;
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
            completionSource.SetResult(e.IsSuccess);
        }
    }
}
