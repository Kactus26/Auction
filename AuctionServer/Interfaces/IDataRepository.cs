using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface IDataRepository
    {
        public Task<User> GetUserDataByid(int id);
    }
}
