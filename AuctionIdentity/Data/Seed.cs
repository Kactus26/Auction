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
            User user4 = new User() { Login = "Grif", Password = "" };
            User user5 = new User() { Login = "Gojo", Password = "" };
            User user6 = new User() { Login = "Makima", Password = "" };

            user1.Password = _passwordHasher.GeneratePassword(user1.Password);
            user2.Password = _passwordHasher.GeneratePassword(user2.Password);
            user3.Password = _passwordHasher.GeneratePassword(user3.Password);
            user4.Password = _passwordHasher.GeneratePassword(user4.Password);
            user5.Password = _passwordHasher.GeneratePassword(user5.Password);
            user6.Password = _passwordHasher.GeneratePassword(user6.Password);

            dataContext.Users.AddRange(user1, user2, user3, user4, user5, user6);
            dataContext.SaveChanges();
        }
    }
}
