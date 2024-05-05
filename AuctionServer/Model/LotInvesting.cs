using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Model
{
    internal class LotInvesting
    {
        public int Id { get; set; }
        public User User { get; set; } = null!;
        public Lot Lot { get; set; } = null!;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public double? Price { get; set; }
    }
}
