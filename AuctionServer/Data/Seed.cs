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


            Lot lot = new Lot() { Name = "Stringi", Description = "Worn 3 months" };

            dataContext.AddRange(lot);
            dataContext.SaveChanges();

        }
    }
}
