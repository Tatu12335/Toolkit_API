namespace Toolkit_API.Domain.Entities.Users
{
    public class UserSession
    {
        public Guid SessionId { get; } = Guid.NewGuid();
        public int UserId { get; init; }
        public string username { get; init; }
        public int isBanned { get; init; } = 0;
        public string role { get; init; }

        public UserSession(string Username, int IsBanned, string Role)
        {
            username = Username;
            isBanned = IsBanned;
            role = Role;
        }


    }
}
