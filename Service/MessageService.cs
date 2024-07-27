
using SMSMessage.Interface;

namespace SMSMessage.Service
{
    public partial class MessageService : IMessageService
    {
        public async Task<bool> RequestSmsPermissionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Sms>();
            return status == PermissionStatus.Granted;
        }

#if WINDOWS
        public async Task<bool> SendMessageAsync(string phoneNumber, string message)
        {
            return false;
        }

        public async Task<List<(string, bool)>> SendMessageAsync(List<string> phoneNumbers, string message)
        {
            return new List<(string, bool)>();
        }
#endif
    }
}
