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
            User user3 = new User() { Name = "Light" };
            User user4 = new User() { Name = "Grif" };
            User user5 = new User() { Name = "Gojo" };
            User user6 = new User() { Name = "Makima" };
            User user7 = new User() { Name = "N" };
            User user8 = new User() { Name = "Dzin" };
            User user9 = new User() { Name = "Tataruta" };
            User zero1 = new User() { Name = "Dumb1" };
            User zero2 = new User() { Name = "Dumb2" };
            User zero3 = new User() { Name = "Dumb3" };
            User zero4 = new User() { Name = "Dumb4" };
            User zero5 = new User() { Name = "Dumb5" };
            User zero6 = new User() { Name = "Dumb6" };
            User zero7 = new User() { Name = "Dumb7" };


            Friendship friendship = new() { User = user1, Friend = user2, Relations = FriendStatus.Friend };
            Friendship friendship2 = new() { User = user3, Friend = user2, Relations = FriendStatus.Friend };
            Friendship friendship3 = new() { User = user2, Friend = user4, Relations = FriendStatus.Blocked };
            Friendship friendship4 = new() { User = user2, Friend = user5, Relations = FriendStatus.Friend };
            Friendship friendship5 = new() { User = user2, Friend = user6, Relations = FriendStatus.Friend };
            Friendship friendship6 = new() { User = user2, Friend = user7, Relations = FriendStatus.Friend };
            Friendship friendship7 = new() { User = user2, Friend = user8, Relations = FriendStatus.Friend };
            Friendship friendship8 = new() { User = user2, Friend = user9, Relations = FriendStatus.Friend };

            Friendship friendship9 = new() { User = user2, Friend = zero1, Relations = FriendStatus.Send };
            Friendship friendship10 = new() { User = user2, Friend = zero2, Relations = FriendStatus.Send };
            Friendship friendship11 = new() { User = user2, Friend = zero3, Relations = FriendStatus.Send };
            Friendship friendship12 = new() { User = user2, Friend = zero4, Relations = FriendStatus.Send };
            Friendship friendship13 = new() { User = user2, Friend = zero5, Relations = FriendStatus.Send };
            Friendship friendship14 = new() { User = user2, Friend = zero6, Relations = FriendStatus.Send };
            Friendship friendship15 = new() { User = user2, Friend = zero7, Relations = FriendStatus.Send };


            user1.InitiatorFriendship.Add(friendship);
            user2.TargetFriendship.Add(friendship2);
            user2.InitiatorFriendship.Add(friendship3);
            user2.InitiatorFriendship.Add(friendship4);
            user2.InitiatorFriendship.Add(friendship5);
            user2.InitiatorFriendship.Add(friendship6);
            user2.InitiatorFriendship.Add(friendship7);
            user2.InitiatorFriendship.Add(friendship8);
            user2.InitiatorFriendship.Add(friendship9);
            user2.InitiatorFriendship.Add(friendship10);
            user2.InitiatorFriendship.Add(friendship11);
            user2.InitiatorFriendship.Add(friendship12);
            user2.InitiatorFriendship.Add(friendship13);
            user2.InitiatorFriendship.Add(friendship14);
            user2.InitiatorFriendship.Add(friendship15);


            dataContext.Users.AddRange(user1, user2, user3, user4, user5, user6, zero1, zero2, zero3, zero4, zero5, zero6, zero7);
            dataContext.SaveChanges();

        }
    }
}
