using AuctionClient.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.Data
{
    internal class DataContext : DbContext
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
