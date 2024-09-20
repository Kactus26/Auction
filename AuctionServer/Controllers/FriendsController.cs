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

        [HttpPost("UnblockUser")]
        public async Task<IActionResult> UnblockUser(UserIdDTO userIdDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            int friendId = userIdDTO.Id;

            var result = await _friendsRepository.RemoveFriend(userId, friendId);

            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                await _friendsRepository.SaveChanges();
                return Ok();
            }
            else
                return BadRequest($"Something went wrong in UnblockUser. {result}");


        }

        [HttpPost("BlockUser")]
        public async Task<IActionResult> BlockUser(UserIdDTO userIdDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            int friendId = userIdDTO.Id;

            Friendship friendship = await _friendsRepository.GetUsersFriendship(userId, friendId);

            if(friendship == null)
            {
                Friendship friends = new() { UserId = userId, FriendId = friendId, Relations = FriendStatus.Blocked, WhoBlockedId = userId };
                await _friendsRepository.AddFriendship(friends);
            }
            else
            {
                friendship.Relations = FriendStatus.Blocked;
                friendship.WhoBlockedId = userId;
            }

            await _friendsRepository.SaveChanges();
            return Ok("User is now blocked");
        }

        [HttpPost("RemoveFriend")]
        public async Task<IActionResult> RemoveFriend(UserIdDTO userIdDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            int friendId = userIdDTO.Id;

            var result = await _friendsRepository.RemoveFriend(userId, friendId);

            if(result.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                await _friendsRepository.SaveChanges();
                return Ok();
            }
            else
                return BadRequest($"Something went wrong in RemoveFriend. {result}");
        }


        [HttpPost("AddFriend")]
        public async Task<IActionResult> AddFriend(UserIdDTO userIdDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            int friendId = userIdDTO.Id;

            Friendship friendship = await _friendsRepository.GetUsersFriendship(userId, friendId);

            if (friendship == null)
            {
                Friendship friends = new() { UserId = userId, FriendId = friendId, Relations = FriendStatus.Send };
                await _friendsRepository.AddFriendship(friends);
                await _friendsRepository.SaveChanges();
                return Ok("Request send");
            }

            if (friendship.Relations == FriendStatus.Send && friendship.UserId != userId)
                friendship.Relations = FriendStatus.Friend;
            else if (friendship.Relations != FriendStatus.Blocked || friendship.Relations != FriendStatus.Friend)
                friendship.Relations = FriendStatus.Send;
            else
                return BadRequest("Something went wrong in AddFriend method");

            await _friendsRepository.SaveChanges();

            return Ok(friendship);
        }

        [HttpPost("GetUsersFriendshipStatus")]
        public async Task<IActionResult> GetUsersFriendshipStatus(UserIdDTO userIdDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            int friendId = userIdDTO.Id;

            Friendship? friendsStatus = await _friendsRepository.GetFriendStatus(userId, friendId);

            if (friendsStatus == null)
                return Ok("Users are not related");

            return Ok(friendsStatus);
        }


        [HttpPost("FindUserInvitations")]
        public async Task<IActionResult> FindUserInvitations(PaginationDTO paginationDTO)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);

            ICollection<User> userInvitations = 
                await _friendsRepository.GetUserInvitations(userId, paginationDTO.CurrentPage, paginationDTO.PageSize);

            if (userInvitations == null)
                return Ok("You have no invitations");

            List<UserDataWithImageDTO> userInvitationsWithImage = UserIntoUserWithImageDTO(userInvitations);

            return Ok(userInvitationsWithImage);
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
