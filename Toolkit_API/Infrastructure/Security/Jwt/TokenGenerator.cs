using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using Toolkit_API.Domain.Entities.Users;
using Toolkit_API.Application.Settings;
using Toolkit_API.Application.Interfaces;
using Microsoft.Extensions.Options;


namespace Toolkit_API.Infrastructure.Security.Jwt
{
    public class TokenGenerator : IGenerateToken
    {
        private readonly JwtSettings _settings;

        public TokenGenerator(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Key);
            


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
                
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
