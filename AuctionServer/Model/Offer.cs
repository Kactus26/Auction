namespace AuctionServer.Model
{
    public class Offer
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public User User { get; set; }
        public Lot Lot {  get; set; }
    }
}
