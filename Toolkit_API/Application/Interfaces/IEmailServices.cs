namespace Toolkit_API.Application.Interfaces
{
    public interface IEmailServices
    {
        public Task<string> SendEmail(string to, string subject, string body);
        public Task<string> SubscribeToNewsLetter(string email);
    }
}
