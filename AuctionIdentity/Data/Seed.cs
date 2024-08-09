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

            User user1 = new User() { Login = "Kactus", Email="sasha.baginsky@gmail.com", Password = "12345"};
            User user2 = new User() { Login = "Odinson", Email="javiest@xdd.com", Password = "52064208" };

            user1.Password = _passwordHasher.GeneratePassword(user1.Password);
            user2.Password = _passwordHasher.GeneratePassword(user2.Password);

            dataContext.Users.AddRange(user1, user2);
            dataContext.SaveChanges();
        }
    }
}
