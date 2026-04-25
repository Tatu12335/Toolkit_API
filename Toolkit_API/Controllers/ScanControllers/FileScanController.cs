using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using Toolkit_API.Application.Application_Services.FileOperations;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.DTOs.FileDTOs;
using Toolkit_API.DTOs.FIleDTOs;
using System.Diagnostics;

namespace Toolkit_API.Controllers.ScanControllers
{
    [EnableRateLimiting("Fixed")]
    [ApiController]
    [Route("api/[controller]")]
    
    public class FileScanController : ControllerBase
    {
        private readonly FileScanOps _fileScanOps;
        private readonly HandleFolder _Handler;
        public FileScanController(FileScanOps fileScanOps ,HandleFolder handle)
        {
            _fileScanOps = fileScanOps;
            _Handler = handle;
        }
        [HttpPost("Scan/File")]
        public async Task<IActionResult> ScanFile([FromBody] FileScanDTO scanDTO)
        {
            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
      
            Debug.WriteLine(userId);
           
            var result = await _fileScanOps.ScanFile(scanDTO.filePath, userId);
            
            Debug.WriteLine(result);
            
            return Ok();
            

        }
        [HttpPost("Scan/Folder")]
        public async Task<IActionResult> ScanFolder([FromBody] FolderScanDTO scanDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _Handler.Handler(scanDTO.filepath,userId);
            return Ok(result);
        }

    }
}
