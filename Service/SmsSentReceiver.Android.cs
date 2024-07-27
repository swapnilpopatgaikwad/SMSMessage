using Android.App;
using Android.Content;

namespace SMSMessage.Service
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { SmsDeliveredAction })]
    public class SmsSentReceiver : BroadcastReceiver
    {
        public const string SmsDeliveredAction = "SMS_DELIVERED_ACTION";
        public static event EventHandler<SmsSentEventArgs> SmsSent;

        public override void OnReceive(Context context, Intent intent)
        {
            string phoneNumber = intent.GetStringExtra("PHONE_NUMBER");
            bool isSuccess = ResultCode == Result.Ok;
            SmsSent?.Invoke(this, new SmsSentEventArgs(phoneNumber, isSuccess));
        }
    }

    public class SmsSentEventArgs : EventArgs
    {
        public bool IsSuccess { get; }
        public string PhoneNumber { get; }

        public SmsSentEventArgs(string phoneNumber, bool isSuccess)
        {
            PhoneNumber = phoneNumber;
            IsSuccess = isSuccess;
        }
    }
}
