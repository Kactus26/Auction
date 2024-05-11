using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Model
{
    internal class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Surname { get; set; } = String.Empty;
        public string Info { get; set; } = String.Empty;
        public string Password { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public double Balance { get; set; } = 0.00;
/*        public ICollection<User> Friends { get; set; }
*/        
        public ICollection<Lot> OwnLots { get; set; } = new List<Lot>();
        public ICollection<Lot> FollowingLots { get; set; } = new List<Lot>();
        public ICollection<LotInvesting> Investings { get; set; } = new List<LotInvesting>();
    }
}
