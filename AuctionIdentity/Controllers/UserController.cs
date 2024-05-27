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
            if(!await _userRepository.CheckUserLogin(request.Login))
            {
                return BadRequest("User with that login already exists");
            }

            string hashedPassword = _passwordHasher.GeneratePassword(request.Password);

            User user = new User
            {
                Login = request.Login,
                Email = request.Email,
                Password = hashedPassword
            };

            await _userRepository.AddUser(user);
            await _userRepository.SaveChanges();

            return Ok();
        }
        [HttpPost("AuthorizeUser")]
        public async Task<IActionResult> AuthorizeUser(AuthUserRequest request)
        {
            if (await _userRepository.CheckUserLogin(request.Login))
            {
                return BadRequest("User with that login doesn't exist");
            }


            return Ok();
        }
    }
}
