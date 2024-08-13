using AuctionServer.Data;
using AuctionServer.Interfaces;
using AuctionServer.Model;
using CommonDTO;
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

        public async Task AddUser(User user)
        {
            using (var transaction = await _dataContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dataContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Users ON");
                    _dataContext.Users.Add(user);
                    await _dataContext.SaveChangesAsync();
                    await _dataContext.Database.ExecuteSqlInterpolatedAsync($"SET IDENTITY_INSERT dbo.Users OFF");
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;

                }

            }

        }

        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
