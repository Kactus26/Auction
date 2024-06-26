﻿using AuctionIdentity.Data;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore;

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

    }
}
