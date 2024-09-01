using AuctionServer.Data;
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

        public async Task<ICollection<User>> GetUserFriendsAndSendStatusByHisId(int userId)
        {
            return await _dataContext.Friendships
                .Where(x => x.FriendId == userId || x.UserId == userId)
                .Where(y=>y.Relations == FriendStatus.Friend || y.Relations == FriendStatus.Send)
                .Select(z=>z.UserId == userId ? z.Friend : z.User)
                .ToListAsync();
        }

/*        public async Task<ICollection<User>> GetUsersByName(string userName)
        {
            return await 
        }*/
    }
}
