using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
﻿using Microsoft.AspNetCore.Mvc;
using Toolkit_API.Application.App_Services.User;
using Toolkit_API.DTOs.UserDTOs;

namespace Toolkit_API.Controllers
{
    [Route("/Login")]
    public class AuthController : ControllerBase
    {
        private readonly Login _login;
        public AuthController(Login login)
        {
            _login = login;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var result = await _login.LoginMethod(loginDTO);
            return Ok(result);
        }
    }
}
