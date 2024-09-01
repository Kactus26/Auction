namespace AuctionServer.Model
{
    public class Friendship
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int FriendId { get; set; }
        public User Friend { get; set; } = null!;

        public FriendStatus Relations { get; set; }
    }

    public enum FriendStatus
    {
        Friend,
        Send,
        Blocked
    }
}
