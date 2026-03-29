using Toolkit_API.Domain.Entities.Users;

namespace Toolkit_API.Application.Interfaces
{
    public interface IGenerateToken
    {
        public string GenerateToken(Users user);


    }
}
