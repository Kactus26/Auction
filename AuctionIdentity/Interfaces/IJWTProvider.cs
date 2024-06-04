using AuctionIdentity.Models;

namespace AuctionIdentity.Interfaces
{
    public interface IJWTProvider
    {
        public string GenerateToken(User user);

    }
}
