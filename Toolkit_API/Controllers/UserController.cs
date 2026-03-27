using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.DTOs.UserDTOs;

namespace Toolkit_API.Controllers
{
    [ApiController]
    [Route("/")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }


        [HttpPost("Create_User")]
        public async Task<IActionResult> CreateUser([FromHeader] CreateUserDTO userDTO)
        {
            await _userRepo.CreateUser(userDTO.username,userDTO.email,userDTO.password);
            return Ok($"User : {userDTO.username}, created");
        }

        [HttpGet("Get_Users")]
        public async Task<IActionResult> GetUserAsync([FromHeader] GetUserDTO userDTO)
        {
            await _userRepo.GetUser(userDTO.username);
            return Ok($"{userDTO.username}");
        }
    }
}
