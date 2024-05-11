using AuctionIdentity.DTO;

namespace AuctionIdentity.Interfaces
{
    public interface IUserRepository
    {
        public Task AddUser(RegisterUserRequest registerUser);
    }
}
