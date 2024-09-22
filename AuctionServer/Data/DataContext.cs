using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendship>()
                .HasKey(uf => new { uf.UserId, uf.FriendId, });
            modelBuilder.Entity<Friendship>()
                .HasOne(fr => fr.User)
                .WithMany(u => u.InitiatorFriendship)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Friendship>()
                .HasOne(fr => fr.Friend)
                .WithMany(u=>u.TargetFriendship)
                .HasForeignKey(u => u.FriendId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany(u => u.OwnLots)
                .WithOne(l => l.Owner);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FollowingLots)
                .WithMany(l => l.Followers);

       

        }
    }
}
