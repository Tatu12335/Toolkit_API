using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Application.Interfaces
{
    public interface IUserRepo
    {
        public Task<Users> CreateUser(string username, string email, byte[] passwordHash, byte[] passwordSalt);
        public Task<Users> GetUser(string username);
        public Task<string> TestConnection();
        public Task<Users> Login(string username, byte[] passwordHash, byte[] passwordSalt);
        
    }
}
