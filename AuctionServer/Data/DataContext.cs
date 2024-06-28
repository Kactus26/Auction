using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace AuctionServer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LotInvesting> LotInvestings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>()
                .HasMany(l => l.OwnLots)
                .WithOne(u => u.Owner);

            modelBuilder.Entity<User>()
                .HasMany(b => b.FollowingLots)
                .WithMany(g => g.Followers);

       

        }
    }
}
