namespace AuctionServer.Model
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Lot> Lot { get; set; } = new List<Lot>();
    }
}
