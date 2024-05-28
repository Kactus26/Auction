using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionIdentity.Services
{
    internal class JWTProvider : IJWTProvider
    {
        private readonly JWTOptions _options;

        public JWTProvider(IOptions<JWTOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(User user)
        {
            Claim[] cliams = [new("userId", user.Id.ToString())];

            var test = _options.SecretKey;

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: cliams,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }

    public class JWTOptions
    {
        public string SecretKey { get; set; } = "thisisverysecretkeythisisverysecretkeythisisverysecretkeythisisverysecretkeythisisverysecretkey";
        public int ExpiresHours { get; set; } = 12;
    }
}
