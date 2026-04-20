using Toolkit_API.Application.Interfaces;
using Toolkit_API.DTOs.UserDTOs;

namespace Toolkit_API.Application.App_Services.User
{
    public class Login
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IGenerateToken _tokenGenerator;
        public Login(IUserRepo userRepo, IPasswordHasher passwordHasher, IGenerateToken generateToken)
        {
            _passwordHasher = passwordHasher;
            _userRepo = userRepo;
            _tokenGenerator = generateToken;
        }
        public async Task<string> LoginMethod(LoginDTO loginDTO)
        {
            var user = await _userRepo.GetUser(loginDTO.username);

            if (user == null)
                throw new UnauthorizedAccessException();

            var isValid = _passwordHasher.VerifyPassword(loginDTO.password, user.passwordHash, user.passwordSalt);

            var isBanned = await CheckBanStatus(loginDTO.username);

            if (isBanned)
                throw new UnauthorizedAccessException("User is banned.");


            if (!isValid)
                throw new UnauthorizedAccessException();

            return _tokenGenerator.GenerateToken(user);
        }
        public async Task<bool> CheckBanStatus(string username)
        {
            var isBanned = await _userRepo.GetBanStatus(username);
            return isBanned;
        }
    }
}
