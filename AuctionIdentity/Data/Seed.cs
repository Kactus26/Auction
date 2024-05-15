using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionIdentity.Data
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

            User user1 = new User() { Login = "Kactus", Email="sasha.baginsky@gmail.com",Password = "1234"};
            User user2 = new User() { Login = "Odinson", Email="javiest@xdd.com", Password = "52064208" };

            dataContext.Users.AddRange(user1, user2);
            dataContext.SaveChanges();
        }
    }
}
