namespace AuctionServer.Model
{
    public class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = String.Empty;
        public double StartPrice { get; set; } = 1;
        public double CurrentPrice { get; set; } = 1;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public User Owner { get; set; } = null!;
        public ICollection<User> Followers { get; set; } = new List<User>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
