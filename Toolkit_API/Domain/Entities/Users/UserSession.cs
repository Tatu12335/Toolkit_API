namespace Toolkit_API.Domain.Entities.Users
{
    public class UserSession
    {
        public Guid SessionId { get; } = Guid.NewGuid();
        public int UserId { get; init; }
        public string username { get; init; }

        public string role { get; init; }

        public UserSession(string Username)
        {

            username = Username;

        }


    }
}
