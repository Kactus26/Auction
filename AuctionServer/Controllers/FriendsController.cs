using AuctionServer.Interfaces;
using AuctionServer.Model;
using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private readonly IFriendsRepository _friendsRepository;

        public FriendsController(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

/*        [HttpPost("FindUser")]
        public async Task<IActionResult> FindUser(string userName)
        {
            ICollection<User> users = await _friendsRepository.GetUsersByName();
        }*/

        [HttpGet("GetUserFriends")]
        public async Task<IActionResult> GetUserFriends()
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            ICollection<User> friends = await _friendsRepository.GetUserFriendsAndSendStatusByHisId(userId);

            if (friends == null)
                return NotFound("Friends not found");

            return Ok(friends);
        }
    }
}
