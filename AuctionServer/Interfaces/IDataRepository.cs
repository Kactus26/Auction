using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface IDataRepository
    {
        public Task<double?> GetUserBalance(int id);
        public Task<User> GetUserDataByid(int id);
        public Task AddUser(User user);
        public Task SaveChanges();

    }
}
