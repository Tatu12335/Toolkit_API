using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Toolkit_API.Controllers.AdminControllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableRateLimiting("Fixed")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
       
     
    }
}
