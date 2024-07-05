using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionIdentity.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> CheckUserLogin(string login);
        public Task<User> GetUserByLogin(string login);
        public ValueTask<EntityEntry<User>> AddUser(User user);
        public Task SaveChanges();
    }
}
