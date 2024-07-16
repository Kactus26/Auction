using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface IDataRepository
    {
        public Task<User> GetUserDataByid(int id);
/*        public ValueTask<User> UpdateUserData(User user);
*/        public Task AddUser(User user);
        public Task SaveChanges();

    }
}
