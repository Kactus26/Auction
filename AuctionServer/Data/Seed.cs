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
            User user2 = new User() { Name = "Odinson" };
            User user3 = new User() { Name = "Light", Surname = "Yagami" };

            Friendship friendship = new() { User = user1, Friend = user2 };
            Friendship friendship2 = new() { User = user3, Friend = user2 };

            user1.InitiatorFriendship.Add(friendship);
            user2.TargetFriendship.Add(friendship2);

            dataContext.Users.AddRange(user1, user2, user3);
            dataContext.SaveChanges();

        }
    }
}
