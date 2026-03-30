using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Toolkit_API.Application.Interfaces;
using Toolkit_API.Domain.Entities.Users;


namespace Toolkit_API.Infrastructure.Security.Jwt
{
    public class TokenGenerator : IGenerateToken
    {
        private readonly string _jwtSecret;

        public TokenGenerator(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public string GenerateToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);



            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
                new Claim(ClaimTypes.Role, user.roles),
                new Claim(ClaimTypes.Name,user.username)

            };



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
