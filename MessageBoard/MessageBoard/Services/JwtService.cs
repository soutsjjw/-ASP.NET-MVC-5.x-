using MessageBoard.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Services
{
    public class JwtService : IJwtService
    {
        private readonly Models.WebConfig _webConfig;

        public JwtService(Models.WebConfig webConfig)
        {
            _webConfig = webConfig;
        }

        public string GenerateToken(string account, string role)
        {
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_webConfig.Jwt.SecretKey);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, account),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = now.AddMinutes(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
