using AuctionServer.Model;
using CommonDTO;

namespace AuctionServer.Interfaces
{
    public interface IDataRepository
    {
        public Task<UserBalanceAndEmailDTO> GetUserBalanceAndEmail(int id);
        public Task<User> GetUserDataByid(int id);
        public Task AddUser(User user);
        public Task SaveChanges();

    }
}
