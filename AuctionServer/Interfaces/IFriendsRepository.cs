using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface IFriendsRepository
    {
        public Task<ICollection<User>> GetUserFriendsAndSendStatusByHisId(int userId);
/*        public Task<ICollection<User>> GetUsersByName(string userName);
*/    }
}
