using AuctionIdentity.DTO;
using AuctionIdentity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserController(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest user)
        {
            string hashedPassword = _passwordHasher.GenereatePassword(user.Password);



            return Ok();
        }

    }
}
