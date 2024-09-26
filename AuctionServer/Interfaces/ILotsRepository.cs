using AuctionServer.Model;

namespace AuctionServer.Interfaces
{
    public interface ILotsRepository
    {
        public Task<ICollection<Lot>> GetUserLotsByIdWithPagination(int userId, int currentPages, int pageSize);
    }
}
