using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionIdentity.Services
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JWTOptions _options;
        private readonly IConfiguration _configuration;

        public JWTProvider(IOptions<JWTOptions> options, IConfiguration configuration)
        {
            _options = options.Value;
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            Claim[] claims = [new("userId", user.Id.ToString())];

            _options.SecretKey = _configuration.GetRequiredSection("JWTOptions").GetValue<string>("SecretKey")!;
            _options.ExpiresHours = _configuration.GetRequiredSection("JWTOptions").GetValue<int>("ExpiresHours");

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }

    public class JWTOptions
    {
        public string SecretKey { get; set; } = null!;
        public int ExpiresHours { get; set; }
    }
}
