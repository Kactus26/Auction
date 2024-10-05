namespace AuctionServer.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public bool IsEmailConfirmed { get; set; } = false;
        public string Info { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public double Balance { get; set; } = 0.00;
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Friendship> InitiatorFriendship { get; set; } = new List<Friendship>();
        public ICollection<Friendship> TargetFriendship { get; set; } = new List<Friendship>();
        public ICollection<Lot> OwnLots { get; set; } = new List<Lot>();
        public ICollection<Lot> FollowingLots { get; set; } = new List<Lot>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
