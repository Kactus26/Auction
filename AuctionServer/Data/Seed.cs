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

/*            User user1 = new User() { Login = "Kactus", Name = "Alex", Surname="Bag", Info="Male, 18 y.o.", Password = "1234"};
            User user2 = new User() { Login = "Odinson", Name = "Yura", Surname = "Bur", Info = "Male, 20 y.o.", Password = "52064208" };
            Lot lot = new Lot() { Name = "Stringi", Description = "Worn 3 months", Owner = user1, Followers = [user2], Comments = [new Comment { Commentator = user2, Text = "Yummy"}] };
            LotInvesting invest = new LotInvesting() { User = user2, Lot = lot, Price = 5 };

            dataContext.AddRange(user1, user2, lot, invest);
            dataContext.SaveChanges();*/

        }
    }
}
