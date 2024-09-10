using AuctionServer.Model;
using CommonDTO;

namespace AuctionServer.Interfaces
{
    public interface IFriendsRepository
    {
        public Task<ICollection<User>> GetUserFriendsByIdWithPagination(int userId, int currentPages, int pageSize);
        public Task<ICollection<User>> GetUsersByName(int? userId, string? name, string? surname, int currentPages, int pageSize);
    }
}
