using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.Model
{
    internal class Comment
    {
        public int Id { get; set; }
        public User Commentator { get; set; }
        public Lot Lot { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
