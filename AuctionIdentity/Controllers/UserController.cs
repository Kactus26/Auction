using AuctionIdentity.DTO;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserController(IPasswordHasher passwordHasher, IUserRepository userRepository)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(string login, string email, string password)
        {
            string hashedPassword = _passwordHasher.GenereatePassword(password);

            User user = new User
            {
                Login = login,
                Email = email,
                Password = hashedPassword
            };

            await _userRepository.AddUser(user);
            await _userRepository.SaveChanges();

            return Ok();
        }

    }
}
