namespace AuctionServer.Model
{
    public class Friendship
    {
        public int UserId { get; set; }
        public User User { get; set; } 

        public int FriendId { get; set; }
        public User Friend { get; set; } 

        public FriendStatus? Relations { get; set; }
    }

    public enum FriendStatus//Сделал тестовую логику для подтверждения но порядок пользователей не учтён
    {
        Friend,
        Send,
        Blocked
    }
}
