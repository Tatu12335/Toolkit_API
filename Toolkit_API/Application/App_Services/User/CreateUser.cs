using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;
using Toolkit_API.DTOs.UserDTOs;

namespace Toolkit_API.Application.App_Services.User
{
    public class CreateUser
    {
        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        public CreateUser(IUserRepo userRepo,IPasswordHasher passwordHasher) 
        { 
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }
        public async Task<Users> Create(CreateUserDTO createUserDTO)
        {
            byte[] saltBytes;
            var passwordHash = _passwordHasher.HashPassword(createUserDTO.password, out saltBytes);
            
            var user = await _userRepo.CreateUser(createUserDTO.username, createUserDTO.email, passwordHash, saltBytes);

            if (user == null)              
                throw new Exception();
            return user;

        }
    }
}
