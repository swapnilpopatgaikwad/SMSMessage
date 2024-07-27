namespace SMSMessage.Interface
{
    public interface IMessageService
    {
        public Task<bool> SendMessageAsync(string phoneNumber, string message);
        Task<List<(string, bool)>> SendMessageAsync(List<string> phoneNumbers, string message);
    }
}
