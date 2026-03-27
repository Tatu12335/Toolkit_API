using System.Runtime.CompilerServices;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Auth;
using Toolkit_API.DTOs.UserDTOs;
using Toolkit_API.Middleware.Exceptions;

namespace Toolkit_API.Application.App_Services.User
{
    public class Login
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IGenerateToken _tokenGenerator;
        public Login(IUserRepo userRepo, IPasswordHasher passwordHasher,IGenerateToken generateToken)
        {
            _passwordHasher = passwordHasher;
            _userRepo = userRepo;
            _tokenGenerator = generateToken;
        }
        public async Task<string> LoginMethod(LoginDTO loginDTO)
        {

            byte[] saltBytes;

            var passwordHash = _passwordHasher.HashPassword(loginDTO.password, out saltBytes); 
            
            var user = await _userRepo.Login(loginDTO.username,passwordHash,saltBytes);

            if (user == null) 
                throw new UnauthorizedAccessException();

            if (!_passwordHasher.VerifyPassword(loginDTO.password, passwordHash, saltBytes)) 
                throw new UnauthorizedAccessException();

            return _tokenGenerator.GenerateToken(user);
        }

    }
}
