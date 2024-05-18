using AuctionIdentity.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;

namespace AuctionIdentity.Repository
{
    internal class UserRepository : IUserRepository
    {
        private DataContext _dataContext;
        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddUser(User user)
        {
            await _dataContext.AddAsync(user);
        }

        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
