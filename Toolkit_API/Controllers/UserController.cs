using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Toolkit_API.Application.App_Services.User;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.DTOs.UserDTOs;

namespace Toolkit_API.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly CreateUser _createUser;

        public UserController(IUserRepo userRepo,CreateUser createUser)
        {
            _userRepo = userRepo;
            _createUser = createUser;
        }


        [HttpPost("Create_User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            await _createUser.Create(userDTO);
            return Ok($"User : {userDTO.username}, created");
        }

        [HttpGet("Get_Users")]
        public async Task<IActionResult> GetUserAsync([FromHeader] GetUserDTO userDTO)
        {
            await _userRepo.GetUser(userDTO.username);
            return Ok($"{userDTO.username}");
        }
        [HttpGet]
        public async Task <IActionResult> TestConnection()
        {
            await _userRepo.TestConnection();
            return Ok("Connection successful");
        }
    }
}
