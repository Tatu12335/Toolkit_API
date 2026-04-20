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
    }
}
