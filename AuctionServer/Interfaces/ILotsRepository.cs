using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface ILotsRepository
    {
        public Task<ICollection<Offer>> GetLotOffersInfo(int lotId);
        public Task<Lot> GetLotSeller(int lotId);
        public Task<ICollection<Lot>> GetUserLotsByIdWithPagination(int userId, int currentPages, int pageSize);
    }
}
