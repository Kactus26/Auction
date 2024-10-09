    namespace AuctionServer.Model
{
    public class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = String.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int StartPrice { get; set; }
        public User Owner { get; set; } = null!;
        public bool IsClosed { get; set; } = false;
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<User> Followers { get; set; } = new List<User>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
