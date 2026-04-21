namespace Toolkit_API.Domain.Entities.Users
{
    public class ForAdminEntity
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; } = string.Empty;
        public string roles { get; set; } = string.Empty;
        
    }
}
