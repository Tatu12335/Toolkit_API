using Toolkit_API.Application.Interfaces;
namespace Toolkit_API.Application.Application_Services.Admin
{
    public class AdminOperations
    {
        private readonly IAdminRepo _adminRepo;
        public AdminOperations(IAdminRepo adminRepo)
        {
            _adminRepo = adminRepo;
        }
        public async Task<string> GetAllUsers()
        {
            var users = await _adminRepo.GetAllUsers();
            if (users == null || !users.Any())
                return "No users found.";
            return string.Join(Environment.NewLine, users);
        }
        public async Task<bool> CheckAdminStatus(int userId)
        {
            return await _adminRepo.CheckAdminStatus(userId);
        }
        public async Task<string> SearchByUsername(string username)
        {
            var result = await _adminRepo.SearchUserByName(username);
            
            if (string.IsNullOrEmpty(result))
                throw new Exception("User not found.");
            
            return result;
        }
    }
}
