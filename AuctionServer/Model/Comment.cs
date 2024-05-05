using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Model
{
    internal class Comment
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
