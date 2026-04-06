using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.DTOs.UserDTOs;
using System.Security.Claims;

namespace Toolkit_API.Controllers.ScanControllers
{
    [EnableRateLimiting("Fixed")]
    [ApiController]
    [Route("/FileOps")]
    [Authorize]
    public class FileScanController : ControllerBase
    {
        private readonly FileScanOps _fileScanOps;
        public FileScanController(FileScanOps fileScanOps)
        {
            _fileScanOps = fileScanOps;
        }
        [HttpPost("Scan")]
        public async Task<IActionResult> ScanFile([FromBody] FileScanDTO scanDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _fileScanOps.ScanFile(scanDTO.filePath,userId);
            return Ok(result);

        }
    }
}
