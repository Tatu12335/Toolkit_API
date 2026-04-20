using Microsoft.AspNetCore.Mvc;
using Toolkit_API.Application.Application_Services.EmailServices;
namespace Toolkit_API.Controllers.EmailControllers
{
    [ApiController]
    [Route("/Emails")]
    public class NewsLetterController : ControllerBase
    {
        private readonly NewLetter _newsLetter;
        public NewsLetterController(NewLetter newsLetter)
        {
            _newsLetter = newsLetter;
        }
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(string email)
        {
            var result = await _newsLetter.Subscribe(email);
            return Ok(result);
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNewsLetter(string to, string subject, string body)
        {
            var result = await _newsLetter.SendNewLetter(to, subject, body);
            return Ok(result);
        }
    }
}
