using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.Model
{
    internal class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Info { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
        public double Balance { get; set; }
/*        public ICollection<User> Friends { get; set; }
*/        
        public ICollection<Lot> OwnLots { get; set; }
        public ICollection<Lot> FollowingLots { get; set; }
        public ICollection<LotInvesting> Investings { get; set; }
    }
}
