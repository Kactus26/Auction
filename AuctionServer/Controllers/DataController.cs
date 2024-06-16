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

            var userDTO = _mapper.Map<UserProfileDTO>(user);

            return Ok(userDTO);
        }
    }
}
class Obj
{    public string Name { get; set; }
    public string Value { get; set; }
}