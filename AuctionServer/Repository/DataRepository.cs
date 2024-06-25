using AuctionServer.Data;
using AuctionServer.Interfaces;
using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionServer.Repository
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _dataContext;

        public DataRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        public async Task<User> GetUserDataByid(int id)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
