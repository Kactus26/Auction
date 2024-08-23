using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionIdentity.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> CheckUserLogin(string login);
        public Task<int> GetUserIdByLogin(string login);
        public Task<User> GetUserByLogin(string login);
        public Task<User> GetUserById(int id);
        public ValueTask<EntityEntry<User>> AddUser(User user);
        public Task SaveChanges();
    }
}
