using Toolkit_API.Application.Interfaces;


namespace Toolkit_API.Application.Application_Services.EmailServices
{
    public class NewLetter
    {
        private readonly IEmailServices _emailServices;

        public NewLetter(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }
        public async Task<string> Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                return "Invalid email address";

            return await _emailServices.SubscribeToNewsLetter(email);
        }
        public async Task<string> SendNewLetter(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to) || !to.Contains("@"))
                return "Invalid email address";

            if (string.IsNullOrEmpty(subject))
                return "Subject cannot be empty";

            if (string.IsNullOrEmpty(body))
                return "Body cannot be empty";

            return await _emailServices.SendEmail(to, subject, body);
        }
    }
}
