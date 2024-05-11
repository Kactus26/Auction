using AuctionIdentity.DTO;
using AuctionIdentity.Interfaces;
using AuctionServer.Model;

namespace AuctionIdentity.Repository
{
    public class UserRepository : IUserRepository
    {


        public async Task AddUser(RegisterUserRequest registerUser)
        {
            User user = new User
            {
                Name = registerUser.UserName,
                Email = registerUser.Email,
                Password = registerUser.Password;
            } 
            _context.Add
        }
    }
}
