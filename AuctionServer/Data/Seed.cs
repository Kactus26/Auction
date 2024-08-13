using AuctionServer.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionServer.Data
{
    internal class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            dataContext = context;
        }

        public async Task SeedDataContext()
        {
            var pendingMigrations = await dataContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await dataContext.Database.MigrateAsync();
            }

            if (dataContext.Users.Any())
            {
                return;
            }


            User user1 = new User() { Name = "Kactus", Email = "sasha.baginsky@gmail.com", IsEmailConfirmed = true };
            User user2 = new User() { Name = "Odinson", Email = "javiest@xdd.com" };

            dataContext.Users.AddRange(user1, user2);
            dataContext.SaveChanges();

        }
    }
}
