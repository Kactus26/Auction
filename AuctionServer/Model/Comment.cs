namespace AuctionServer.Model
{
    public class Comment
    {
        public int Id { get; set; }
        public User Commentator { get; set; } = null!;
        public Lot Lot { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;
    }
}