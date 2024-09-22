namespace AuctionServer.Model
{
    public class LotInvesting
    {
        public int Id { get; set; }
        public User Investor { get; set; } = null!;
        public Lot Lot { get; set; } = null!;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public double? Price { get; set; }
    }
}
