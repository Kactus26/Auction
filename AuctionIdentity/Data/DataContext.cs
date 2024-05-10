using AuctionIdentity.Model;
using Microsoft.EntityFrameworkCore;


namespace AuctionIdentity.Data
{
    internal class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }


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
