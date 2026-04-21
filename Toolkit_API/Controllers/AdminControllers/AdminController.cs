using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toolkit_API.Application.Application_Services.Admin;
namespace Toolkit_API.Controllers.AdminControllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("Fixed")]
    public class AdminController : ControllerBase
    {
        private readonly AdminOperations _adminOperations;

        public AdminController(AdminOperations adminOperations)
        {
            _adminOperations = adminOperations;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _adminOperations.GetAllUsers();
            return Ok(result);
        }
        [HttpGet("SearchByUsername")]
        public async Task<IActionResult> SearchByUName(string username)
        {
            var result = await _adminOperations.SearchByUsername(username);
            return Ok(result);
        }
    }
}
