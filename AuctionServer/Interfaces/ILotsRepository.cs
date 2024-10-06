using AuctionServer.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AuctionServer.Interfaces
{
    public interface ILotsRepository
    {
        public Task<Lot> GetLotById(int lotId);
        public Task<User> GetUserById(int userId);
        public Task<EntityEntry<Offer>> AddOffer(Offer offer);
        public Task<ICollection<Offer>> GetLotOffersInfo(int lotId);
        public Task<Lot> GetLotSeller(int lotId);
        public Task<ICollection<Lot>> GetUserLotsByIdWithPagination(int userId, int currentPages, int pageSize);
        public Task SaveChanges();
    }
}
