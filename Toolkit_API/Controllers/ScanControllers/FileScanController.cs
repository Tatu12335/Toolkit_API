using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Toolkit_API.Controllers.ScanControllers
{
    [ApiController]
    [Route("/File_Scan")]
    [Authorize]
    public class FileScanController : ControllerBase
    {
       
    }
}
