using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Application.Interfaces
{
    public interface IAdminRepo
    {
        public Task<IEnumerable<ForAdminEntity>> GetAllUsers();
        public Task<bool> CheckAdminStatus(int userId);
        public Task<bool> CheckUserExists(int userId);
        public Task<string> GetUserEmail(int userId);
        public Task<string> GetUsername(int userId);
        public Task<string> GetUserRole(int userId);
        public Task<int> DeleteUser(int userId);
        public Task<int> UpdateUserRole(int userId, string newRole);
        public Task<int> UpdateUserEmail(int userId, string newEmail);
        public Task<int> UpdateUsername(int userId, string newUsername);
        public Task<string> SearchUserByName(string username);
        public Task<int> DeleteUserByName(string username);
        public Task<bool> SearchActiveUsers(string username);

    }
}
