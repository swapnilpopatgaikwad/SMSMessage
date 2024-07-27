namespace SMSMessage.Interface
{
    public interface IMessageService
    {
        public Task<bool> SendMessageAsync(string phoneNumber, string message);
    }
}
