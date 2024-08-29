using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionServer.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public bool IsEmailConfirmed { get; set; } = false;
        public string Info { get; set; } = String.Empty;
        public string? ImageUrl { get; set; }
        public double Balance { get; set; } = 0.00;
        public ICollection<Friendship> InitiatorFriendship { get; set; } = new List<Friendship>();
        public ICollection<Friendship> TargetFriendship { get; set; } = new List<Friendship>();
        public ICollection<Lot> OwnLots { get; set; } = new List<Lot>();
        public ICollection<Lot> FollowingLots { get; set; } = new List<Lot>();
        public ICollection<LotInvesting> Investings { get; set; } = new List<LotInvesting>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
