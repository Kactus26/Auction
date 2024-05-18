using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using Common.DTO;
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
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            string hashedPassword = _passwordHasher.GenereatePassword(request.Password);

            User user = new User
            {
                Login = request.UserName,
                Email = request.Email,
                Password = hashedPassword
            };

            await _userRepository.AddUser(user);
            await _userRepository.SaveChanges();

            return Ok();
        }
    }
}
