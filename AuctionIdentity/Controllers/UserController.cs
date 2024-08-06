using AuctionIdentity.Interfaces;
using AuctionIdentity.Models;
using CommonDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionIdentity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMailService _mailService;
        private readonly IJWTProvider _jwtProvider;

        public UserController(IPasswordHasher passwordHasher, IUserRepository userRepository, IJWTProvider jwtProvider, IMailService mailService)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _mailService = mailService;
        }

        [HttpPost("SendEmail")]
        public IActionResult SendEmail(RegisterUserRequest request) 
        {
            string result = _mailService.SendEmail(request.Email);
            return Ok(result);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            /*if (!await _userRepository.CheckUserEmail(request.Email))
                return BadRequest("User with this email already exists");*/

            if (!await _userRepository.CheckUserLogin(request.Login))
            {
                return BadRequest("User with this login already exists");
            }

            string hashedPassword = _passwordHasher.GeneratePassword(request.Password);

            User user = new User
            {
                Login = request.Login,
                Email = request.Email,
                Password = hashedPassword
            };

            var result = await _userRepository.AddUser(user);
            if (result.State != EntityState.Added)
                return BadRequest("User was't added");

            await _userRepository.SaveChanges();

            string token = _jwtProvider.GenerateToken(user);

            return Ok(token);
        }

        [HttpPost("AuthorizeUser")]
        public async Task<IActionResult> AuthorizeUser(AuthUserRequest request)
        {
            User user = await _userRepository.GetUserByLogin(request.Login);

            if(user == null)
            {
                return BadRequest("User with that login doesn't exist");
            } 
            else if (!_passwordHasher.Verify(request.Password, user.Password))
            {
                return BadRequest("Password is incorrect");
            }

            string token = _jwtProvider.GenerateToken(user);

            return Ok(token);
        }
    }
}
