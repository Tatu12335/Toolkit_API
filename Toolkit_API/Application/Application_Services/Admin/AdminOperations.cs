using System.Diagnostics;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;
namespace Toolkit_API.Application.Application_Services.Admin
{
    public class AdminOperations
    {
        private readonly IAdminRepo _adminRepo;
        public AdminOperations(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        public async Task<List<string>> GetAllUsers()
        {
            var users = await _adminRepo.GetAllUsers();
            List<string> result = new List<string>();
            if (users == null || users.Count() == 0)
                throw new Exception("No users found.");
            foreach (var user in users)
            {
                Debug.WriteLine($"ID: {user.id}, Username: {user.username}, Email: {user.newemail}, Roles: {user.roles}");
                result.Add($"ID: {user.id}, Username: {user.username}, Email: {user.newemail}, Roles: {user.roles}");
            }
            return result;
        }
        public async Task<bool> CheckAdminStatus(int userId)
        {
            return await _adminRepo.CheckAdminStatus(userId);
        }
        public async Task<ForAdminEntity> SearchByUsername(string username)
        {
            var result = await _adminRepo.SearchUserByName(username);

            if (result == null)
                throw new Exception("User not found.");

            return result;
        }
        public async Task<string> DeleteUserByUsername(string username)
        {
            var result = await _adminRepo.DeleteUserByName(username);

            if (result == 0)
                throw new Exception("User not found or could not be deleted.");

            return "User deleted successfully.";
        }
        public async Task<bool> SearchActiveUsers(string username)
        {
            return await _adminRepo.SearchActiveUsers(username);
        }
    }
}
