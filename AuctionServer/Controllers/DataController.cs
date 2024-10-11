using AuctionServer.Interfaces;
using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : Controller//Shoud've called it UserDataController 
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
                

        [HttpGet("GetUserBalanceAndEmail")]
        public async Task<IActionResult> GetUserBalanceAndEmail()
        {
            int userId = Convert.ToInt32(User.Identities.First().Claims.First().Value);

            UserBalanceAndEmailDTO userDTO = await _dataRepository.GetUserBalanceAndEmail(userId);
            userDTO.UserId = userId;

            if (userDTO == null)
                return NotFound("Something not found");

            return Ok(userDTO);
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

                string imgUrl = user.ImageUrl;

                if(user == null) 
                    return NotFound("User not found");

                user = _mapper.Map(newData.ProfileData, user);
                user.Id = userId;

                if(newData.Image != null)
                    UploadImage(newData.Image, user);
                else
                    user.ImageUrl = imgUrl;//imageUrl generates only in UploadImage(), that's why need to set it manually

                await _dataRepository.SaveChanges();

                return Ok("Data updated");
        }

        private IActionResult UploadImage(byte[] image, User user)
        {
            try
            {
                var uploadPath = Path.Combine(_environment.WebRootPath, "userImages");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, System.Convert.ToString(user.Id) + ".jpg");

                if (Path.Exists(filePath))//Deletes previous user image
                    System.IO.File.Delete(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Write(image);
                }

                user.ImageUrl = filePath;

                return Ok("User Image successfully updated");

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}