using AuctionServer.Data;
using AuctionServer.Interfaces;
using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionServer.Repository
{
    public class LotsRepository : ILotsRepository
    {
        private readonly DataContext _dataContext;

        public LotsRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<Lot> GetLotById(int lotId)
        {
            return await _dataContext.Lots.FirstOrDefaultAsync(l=>l.Id == lotId);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<ICollection<Offer>> GetLotOffersInfo(int lotId)
        {
            return await _dataContext.Offers
                .Include(x=>x.User)
                .Where(x => x.Lot.Id == lotId)
                .OrderByDescending(p=>p.Price)
                .Take(10)
                .ToListAsync();
        }

        public async Task<Lot> GetLotWithSeller(int lotId)
        {
            return await _dataContext.Lots
                .Include(x=>x.Owner)
                .FirstOrDefaultAsync(x=>x.Id==lotId);
        }

        public async Task<ICollection<Lot>> GetLotsByNameWithPagination(string name, int currentPages, int pageSize)
        {
            return await _dataContext.Lots
                        .Where(u => (name != null ? u.Name.Contains(name) : true))
                        .OrderBy(d => d.DateTime)
                        .Skip((currentPages - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public async Task<ICollection<Lot>> GetUserLotsByIdWithPagination(int userId, int currentPages, int pageSize)
        {
            return await _dataContext.Lots
                    .Where(x => x.Owner.Id == userId)
                    .OrderBy(d => d.DateTime)
                    .Skip((currentPages - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        }

        public async Task<ICollection<Lot>> GetLotsWithPagination(int currentPages, int pageSize)
        {
            return await _dataContext.Lots
                .OrderBy(d => d.DateTime)
                .Skip((currentPages - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<EntityEntry<Offer>> AddOffer(Offer offer)
        {
            return await _dataContext.Offers.AddAsync(offer);
        }

        public async Task SaveChanges()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task<EntityEntry<Lot>> AddLot(Lot lot)
        {
            return await _dataContext.Lots.AddAsync(lot);
        }


    }
}
