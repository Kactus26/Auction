using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;

namespace Auction.Tests.Data
{
    internal class DataContext : DbContext
    {
        private readonly DbContextOptions _options;
        public DataContext(DbContextOptions options)
            : base(options)
        {
            _options = options;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LotInvesting> LotInvestings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestingDb");
        }

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
