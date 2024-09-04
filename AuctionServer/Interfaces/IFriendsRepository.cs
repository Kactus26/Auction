using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface IFriendsRepository
    {
        public Task<ICollection<User>> GetUserFriendsByIdWithPagination(int userId, int currentPages, int pageSize);
/*        public Task<ICollection<User>> GetUsersByName(string userName);
*/    }
}
