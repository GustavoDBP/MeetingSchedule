using MeetingSchedule.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using MeetingSchedule.Models;
using Microsoft.Extensions.Options;

namespace MeetingSchedule.Services
{
    public interface IAuthenticationService
    {
        string GenerateToken(User user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOptions<AuthenticationSettings> _authenticationSettings;

        public AuthenticationService(IOptions<AuthenticationSettings> authenticationSettings)
        {
            _authenticationSettings = authenticationSettings;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationSettings.Value.JWTSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
