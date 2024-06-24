using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IMapper _mapper;
        public string UserId => User.Identities.First().Claims.First().Value;

        public DataController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData(UserProfileDTO pr)
        {
            


            var user = new User { Name = "Sasha" };
            var test = _mapper.Map<UserProfileDTO>(user);

            

            return Ok(test);
        }
    }
}
