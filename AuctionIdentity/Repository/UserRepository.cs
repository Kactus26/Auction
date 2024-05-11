using AuctionIdentity.Data;
using AuctionIdentity.DTO;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;

namespace AuctionIdentity.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddUser(RegisterUserRequest registerUser)
        {
            /*User user = new User
            {
                Login = registerUser.UserName,
                Email = registerUser.Email,
                Password = registerUser.Password
            };
            _dataContext.Add(user);
            _dataContext.SaveChanges();*/
        }
    }
}
