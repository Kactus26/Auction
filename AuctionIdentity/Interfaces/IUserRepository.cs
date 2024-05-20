using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionIdentity.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> CheckUserLogin(string login);
        public Task AddUser(User user);
        public Task SaveChanges();
    }
}
