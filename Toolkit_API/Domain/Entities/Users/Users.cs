namespace Toolkit_API.Domain.Entities.Users
{
    public class Users
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}
