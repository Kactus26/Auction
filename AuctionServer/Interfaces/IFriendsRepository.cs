using AuctionServer.Model;
using CommonDTO;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionServer.Interfaces
{
    public interface IFriendsRepository
    {
        public ValueTask<EntityEntry<Friendship>> AddFriendship(Friendship friendship);
        public Task<Friendship> GetUsersFriendship(int userId, int friendId);
        public Task<ICollection<User>> GetUserFriendsByIdWithPagination(int userId, int currentPages, int pageSize);
        public Task<ICollection<User>> GetUsersByName(int? userId, string? name, string? surname, int currentPages, int pageSize);
        public Task<ICollection<User>> GetUserInvitations(int userId, int currentPages, int pageSize);
        public Task<EntityEntry<Friendship>> RemoveFriend(int userId, int friendId);
        public Task<Friendship?> GetFriendStatus(int id, int anotherUserId);
        public Task SaveChanges();
    }
}
