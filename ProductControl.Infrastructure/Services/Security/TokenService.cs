using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductControl.Domain.Entities;
using ProductControl.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductControl.Infrastructure.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;
        private readonly int _minutes;

        public TokenService(IConfiguration config)
        {
            var section = config.GetSection("Jwt");
            _issuer = section["Issuer"]!;
            _audience = section["Audience"]!;
            _key = section["Key"]!;
            _minutes = int.Parse(section["AccessTokenMinutes"] ?? "60");
        }

        public string TokeGenerator(Users user, string role)
        {
            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
            SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_minutes),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
