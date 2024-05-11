using AuctionIdentity.DTO;
using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionIdentity.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(User user);
        public Task SaveChanges();
    }
}
