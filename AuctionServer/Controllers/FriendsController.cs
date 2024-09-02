using AuctionServer.Interfaces;
using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly IMapper _mapper;

        public FriendsController(IFriendsRepository friendsRepository, IMapper mapper)
        {
            _friendsRepository = friendsRepository;
            _mapper = mapper;
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

            List<UserDataWithImageDTO> friendsWithImages = new List<UserDataWithImageDTO>();

            foreach(User user in friends)
            {
                UserProfileDTO userData = _mapper.Map<UserProfileDTO>(user);
                UserDataWithImageDTO friendWithImage = new UserDataWithImageDTO() { ProfileData = userData };

                if(user.ImageUrl != null)
                {
                    byte[] image = System.IO.File.ReadAllBytes(user.ImageUrl);
                    friendWithImage.Image = image;
                }

                friendsWithImages.Add(friendWithImage);
            }

            return Ok(friendsWithImages);
        }
    }
}
