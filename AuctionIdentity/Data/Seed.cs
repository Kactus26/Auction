using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionIdentity.Data
{
    internal class Seed
    {
        private readonly DataContext dataContext;
        private readonly IPasswordHasher _passwordHasher;
        public Seed(DataContext context, IPasswordHasher passwordHasher)
        {
            dataContext = context;
            _passwordHasher = passwordHasher;
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

            User user1 = new User() { Login = "Kactus", Password = "12345"};
            User user2 = new User() { Login = "Odinson", Password = "11111" };
            User user3 = new User() { Login = "Kira", Password = "" };

            user1.Password = _passwordHasher.GeneratePassword(user1.Password);
            user2.Password = _passwordHasher.GeneratePassword(user2.Password);
            user3.Password = _passwordHasher.GeneratePassword(user3.Password);

            dataContext.Users.AddRange(user1, user2, user3);
            dataContext.SaveChanges();
        }
    }
}
