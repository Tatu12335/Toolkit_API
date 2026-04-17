using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toolkit_API.Application.Application_Services.Operations;
using Toolkit_API.DTOs.UserDTOs;

namespace Toolkit_API.Controllers.FileAnalysisControllers
{
    [ApiController]
    [Route("/FileOps/Analysis")]
    [Authorize]
    public class AnalyzeFileController : ControllerBase
    {
        private readonly StaticFileAnalysis _fileAnalysis;
        public AnalyzeFileController(StaticFileAnalysis fileAnalysis)
        {
            _fileAnalysis = fileAnalysis;
        }
        [HttpPost("Analyze")]
        public async Task<IActionResult> AnalyzeFile([FromBody] FileAnalysisDTO fileDTO)
        {
            var result = await _fileAnalysis.AnalyzeFile(fileDTO.FilePath);
            return Ok(result);

        }
    }
}
