using AuctionServer.Data;
using AuctionServer.Interfaces;
using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionServer.Repository
{
    public class LotsRepository : ILotsRepository
    {
        private readonly DataContext _dataContext;

        public LotsRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<ICollection<Offer>> GetLotOffersInfo(int lotId)
        {
            return await _dataContext.Offers
                .Include(x=>x.User)
                .Where(x => x.Lot.Id == lotId)
                .Take(10)
                .ToListAsync();
        }

        public async Task<Lot> GetLotSeller(int lotId)
        {
            return await _dataContext.Lots
                .Include(x=>x.Owner)
                .FirstOrDefaultAsync(x=>x.Id==lotId);
        }

        public async Task<ICollection<Lot>> GetUserLotsByIdWithPagination(int userId, int currentPages, int pageSize)
        {
            return await _dataContext.Lots
                    .Where(x => x.Owner.Id == userId)
                    .Skip((currentPages - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }
    }
}
