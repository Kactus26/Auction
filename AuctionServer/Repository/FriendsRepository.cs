﻿using AuctionServer.Data;
using AuctionServer.Interfaces;
using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionServer.Repository
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly DataContext _dataContext;

        public FriendsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<FriendStatus?> GetFriendStatus(int id, int anotherUserId)
        {
            return await _dataContext.Friendships
                .Where(x => x.UserId == id || x.UserId == anotherUserId)
                .Where(y => y.FriendId == anotherUserId || y.FriendId == id)
                .Select(z => z.Relations)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<User>> GetUserFriendsByIdWithPagination(int userId, int currentPages, int pageSize)
        {
            return await _dataContext.Friendships
                .Where(x => x.FriendId == userId || x.UserId == userId)
                .Where(y => y.Relations == FriendStatus.Friend)
                .Select(z => z.UserId == userId ? z.Friend : z.User)
                .Skip((currentPages - 1) * pageSize)
                .Take(pageSize)                      
                .ToListAsync();
        }

        public async Task<ICollection<User>> GetUserInvitations(int userId, int currentPages, int pageSize)
        {
            return await _dataContext.Friendships
                            .Where(x => x.FriendId == userId || x.UserId == userId)
                            .Where(y => y.Relations == FriendStatus.Send)
                            .Select(z => z.UserId == userId ? z.Friend : z.User)
                            .Skip((currentPages - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }

        public async Task<ICollection<User>> GetUsersByName(int? userId, string? name, string? surname, int currentPages, int pageSize)
        {
            if(userId != null)
                return await _dataContext.Users
                    .Where(u => (name != null ? u.Name.Contains(name) : true) 
                    && (surname != null ? u.Surname.Contains(surname) : true)
                    && !_dataContext.Friendships.Any(f =>
                            (f.UserId == userId && f.FriendId == u.Id ||
                             f.FriendId == userId && f.UserId == u.Id)
                            && f.Relations == FriendStatus.Blocked))
                    .Skip((currentPages - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            else
                return await _dataContext.Users
                    .Where(u => (name != null ? u.Name.Contains(name) : true)
                    && (surname != null ? u.Surname.Contains(surname) : true))
                    .Skip((currentPages - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }
    }
}
