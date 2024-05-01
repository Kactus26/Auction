using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.Model
{
    internal class LotInvesting
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Lot Lot { get; set; }
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
    }
}
