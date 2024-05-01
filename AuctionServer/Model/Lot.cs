using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.Model
{
    internal class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double StartPrice { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime DateTime { get; set; }
        public User Owner { get; set; }
        public ICollection<User> Followers { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
