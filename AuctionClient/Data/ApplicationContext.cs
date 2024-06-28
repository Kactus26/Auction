using Microsoft.EntityFrameworkCore;

namespace AuctionClient.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<LoggedUser> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=loggedUser.db");
            Чтоб не втыкал
        }
    }
}
