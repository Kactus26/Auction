using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface IFriendsRepository
    {
        public Task<ICollection<User>> GetUserFriendsByHisId(int userId);
    }
}
