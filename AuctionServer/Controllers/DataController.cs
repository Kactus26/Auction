using AuctionServer.Interfaces;
using AuctionServer.Model;
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
        private readonly IWebHostEnvironment _environment;

        public string UserId => User.Identities.First().Claims.First().Value;

        public DataController(IMapper mapper, IDataRepository dataRepos, IWebHostEnvironment environment)
        {
            _dataRepository = dataRepos;
            _mapper = mapper;
            _environment = environment;
        }

        [HttpPost("IsEmailConfirmed")]//Returns IsEmailConfirmed field
        public async Task<IActionResult> IsEmailConfirmed(UserIdDTO userId)
        {
            User user = await _dataRepository.GetUserDataByid(userId.Id);

            if(user == null)
                return NotFound("The user with this id doesn't exist");

            if (user.IsEmailConfirmed == false)
                return BadRequest("The user with this id has not confirmed his email. If it's really your account, contact our support and don't be afraid!");

            return Ok(user.Email);
        }

        [HttpGet("EmailIsConfirmed")]//Changes IsEmailConfirmed field
        public async Task<IActionResult> EmailIsConfirmed()
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = await _dataRepository.GetUserDataByid(userId);

            if (user == null)
                return NotFound("User not found");

            user.IsEmailConfirmed = true;
            await _dataRepository.SaveChanges();

            return Ok("Email is successfully confirmed");
        }

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = await _dataRepository.GetUserDataByid(userId);

            if (user == null)
                return NotFound();

            UserProfileDTO userDTO = _mapper.Map<UserProfileDTO>(user);

            UserDataWithImageDTO userDataWithImageDTO = new UserDataWithImageDTO { ProfileData = userDTO };

            if (userDTO.ImageUrl != null)
            {
                byte[] image = System.IO.File.ReadAllBytes(userDTO.ImageUrl);

                userDataWithImageDTO.Image = image;
            }

            return Ok(userDataWithImageDTO);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser(RegisterUserRequest userRequest)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = _mapper.Map<User>(userRequest);
            user.Id = userId;
            user.Name = "New User";

            await _dataRepository.AddUser(user);

            return Ok("User has been added");
        }

        [HttpPost("UpdateUserData")]
        public async Task<IActionResult> UpdateUserData(UserDataWithImageDTO newData)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = await _dataRepository.GetUserDataByid(userId);

            if(user == null) 
                return NotFound("User not found");

            user = _mapper.Map(newData.ProfileData, user);
            user.Id = userId;

            if(newData.Image != null)
            {
                UploadImage(newData.Image, user);
            }

            await _dataRepository.SaveChanges();

            return Ok();
        }

        private IActionResult UploadImage(byte[] image, User user)
        {            
            var uploadPath = Path.Combine(_environment.WebRootPath, "userImages");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, System.Convert.ToString(user.Id) + ".jpg");

            if(Path.Exists(filePath))//Deletes previous user image
                System.IO.File.Delete(filePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                stream.Write(image);
            }

            user.ImageUrl = filePath;

            return Ok("User Image successfully updated");
        }
    }
}