using AuctionServer.Interfaces;
using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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

        [HttpPost("FindUser")]
        public async Task<IActionResult> FindUser(PaginationUserSearchDTO userSearchDTO)
        {
            int? userId = null;

            try
            {
                userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            } catch(Exception ex)
            {
                //If user have exception here, it means that he has no token and he is guest;
            }

            ICollection<User> searchResult = await _friendsRepository.GetUsersByName(userId, userSearchDTO.Name, userSearchDTO.Surname, userSearchDTO.CurrentPage, userSearchDTO.PageSize);

            if (searchResult == null)
                return NotFound("Users with this name & surname don't exists");

            List<UserDataWithImageDTO> usersFound = UserIntoUserWithImageDTO(searchResult); 

            return Ok(usersFound);
        }

        [HttpPost("GetUserFriends")]
        public async Task<IActionResult> GetUserFriends(PaginationDTO paginationDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            ICollection<User> friends = await _friendsRepository.GetUserFriendsByIdWithPagination(userId, paginationDTO.CurrentPage, paginationDTO.PageSize);

            if (friends == null)
                return NotFound("Friends not found");

            List<UserDataWithImageDTO> friendsWithImages = UserIntoUserWithImageDTO(friends);

            return Ok(friendsWithImages);
        }

        private List<UserDataWithImageDTO> UserIntoUserWithImageDTO(ICollection<User> friends)
        {
            List<UserDataWithImageDTO> friendsWithImages = new List<UserDataWithImageDTO>();

            foreach (User user in friends)
            {
                UserProfileDTO userData = _mapper.Map<UserProfileDTO>(user);
                UserDataWithImageDTO friendWithImage = new UserDataWithImageDTO() { ProfileData = userData };

                if (user.ImageUrl != null)
                {
                    byte[] image = System.IO.File.ReadAllBytes(user.ImageUrl);
                    friendWithImage.Image = image;
                }

                friendsWithImages.Add(friendWithImage);
            }
            return friendsWithImages;
        }
    }
}
