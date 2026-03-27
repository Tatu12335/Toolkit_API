using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Application.Interfaces
{
    public interface IUserRepo
    {
        public Task <UserSession> CreateUser(string username,string email, string password);
        public Task<Users> GetUser(string username);
    }
}
