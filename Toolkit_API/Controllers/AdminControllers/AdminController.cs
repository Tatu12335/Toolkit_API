using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toolkit_API.Application.Application_Services.Admin;
namespace Toolkit_API.Controllers.AdminControllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("Fixed")]
    [Authorize(Roles = "Admin")]
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
        [HttpPost("SearchByUsername")]
        public async Task<IActionResult> SearchByUName([FromBody] string username)
        {
            var result = await _adminOperations.SearchByUsername(username);
            return Ok(result);
        }
    }
}
