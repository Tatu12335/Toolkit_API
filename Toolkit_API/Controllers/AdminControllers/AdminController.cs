using Microsoft.AspNetCore.Mvc;
using Toolkit_API.Application.Application_Services.Admin;
namespace Toolkit_API.Controllers.AdminControllers
{
    public class AdminController : Controller
    {
        private readonly AdminOperations _adminOperations;

        public AdminController(AdminOperations adminOperations)
        {
            _adminOperations = adminOperations;
        }
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _adminOperations.GetAllUsers();
            return Ok(result);
        }
        public async Task<IActionResult> SearchByUName(string username)
        {
            var result = await _adminOperations.SearchByUsername(username);
            return Ok(result);
        }
}
