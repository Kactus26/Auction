using AuctionIdentity.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AuctionIdentity.Services
{
    public class PasswordHasherService : IPasswordHasher
    {
        public string GenereatePassword(string password) 
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password); 
        public bool Verify(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
