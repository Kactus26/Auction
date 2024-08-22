using AuctionIdentity.Data;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionIdentity.Repository
{
    internal class UserRepository : IUserRepository
    {
        private DataContext _dataContext;
        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async ValueTask<EntityEntry<User>> AddUser(User user)
        {
            return await _dataContext.AddAsync(user);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> CheckUserLogin(string login)
        {
            if(await _dataContext.Users.Where(x=>x.Login == login).FirstOrDefaultAsync() == null)
                return true;
            else
                return false;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }

        public Task<int> GetUserIdByLogin(string login)
        {
            return _dataContext.Users
                .Where(x=>x.Login==login)
                .Select(x=>x.Id)
                .FirstOrDefaultAsync();
        }
    }
}
