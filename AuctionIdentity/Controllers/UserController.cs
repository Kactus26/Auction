using AuctionIdentity.DTO;
using AuctionIdentity.Interfaces;
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


        /*[HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser()
        {
            string hashedPassword = _passwordHasher.GenereatePassword(user.Password);
            _userRepository.AddUser(user);


            return Ok();
        }*/

    }
}
