using AuctionServer.Interfaces;
using AuctionServer.Model;
using AuctionServer.Repository;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        public string UserId => User.Identities.First().Claims.First().Value;

        public DataController(IMapper mapper, IDataRepository dataRepos)
        {
            _dataRepository = dataRepos;
            _mapper = mapper;
        }

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = await _dataRepository.GetUserDataByid(userId);

            if(user == null)
                return NotFound();

            var userDTO = _mapper.Map<UserProfileDTO>(user);

            return Ok(userDTO);
        }
        
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(RegisterUserRequest userRequest)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = _mapper.Map<User>(userRequest);
            user.Id = userId;
            user.Name = "New User)";

            await _dataRepository.AddUser(user);

            return Ok("User has been added");
        }
    }
}