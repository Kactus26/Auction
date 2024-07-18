using AuctionServer.Interfaces;
using AuctionServer.Model;
using AuctionServer.Repository;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpGet("GetUserData")]
        public async Task<IActionResult> GetUserData()
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = await _dataRepository.GetUserDataByid(userId);

            if(user == null)
                return NotFound();

            UserProfileDTO userDTO = _mapper.Map<UserProfileDTO>(user);

            return Ok(userDTO);
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
        public async Task<IActionResult> UpdateUserData(ChangedDataDTO newData)
        {
            int userId = System.Convert.ToInt32(User.Identities.First().Claims.First().Value);
            User user = await _dataRepository.GetUserDataByid(userId);

            if(user == null) 
                return NotFound("User not found");

            user = _mapper.Map(newData, user);

            await _dataRepository.SaveChanges();

            return Ok();
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadPath = Path.Combine(_environment.WebRootPath, "userImages");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { FilePath = filePath });
        }
    }
}