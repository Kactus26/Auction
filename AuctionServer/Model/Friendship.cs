namespace AuctionServer.Model
{
    public class Friendship
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int FriendId { get; set; }
        public User Friend { get; set; }

        public FriendStatus? Relations { get; set; }
        public int? WhoBlockedId { get; set;}
    }

    public enum FriendStatus
    {
        Friend,
        Send,
        Blocked
    }
}
