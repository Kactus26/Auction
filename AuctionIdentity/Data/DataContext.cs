using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore;


namespace AuctionIdentity.Data
{
    internal class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
