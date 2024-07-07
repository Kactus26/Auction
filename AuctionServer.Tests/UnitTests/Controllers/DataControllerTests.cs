using AuctionServer.Controllers;
using AuctionServer.Interfaces;
using AuctionServer.Model;
using AutoMapper;
using CommonDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace AuctionServer.Tests.UnitTests.Controllers
{
    public class DataControllerTests
    {
        private readonly Mock<IDataRepository> _mockDataRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly DataController _dataController;

        public DataControllerTests()
        {
            _mockDataRepository = new Mock<IDataRepository>();
            _mockMapper = new Mock<IMapper>();

            _dataController = new DataController(_mockMapper.Object, _mockDataRepository.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));

            _dataController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

        }

        [Fact]
        public async Task DataController_UserNotFound_ReturnNotFound()
        {
            //Arrange
            var userId = 1;
            var user = new User { Id = userId, Name = "Test User" };
            var userDTO = new UserProfileDTO { Id = userId, Name = "Test User" };
            _mockDataRepository.Setup(x => x.GetUserDataByid(It.IsAny<int>())).ReturnsAsync((User?)null);
            _mockMapper.Setup(m => m.Map<UserProfileDTO>(user)).Returns(userDTO);

            // Act
            IActionResult result = await _dataController.GetUserData();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DataController_CheckResult_ReturnOk()
        {
            //Arrange
            var userId = 1;
            var user = new User { Id = userId, Name = "Test User" };
            var userDTO = new UserProfileDTO { Id = userId, Name = "Test User" };
            _mockDataRepository.Setup(x => x.GetUserDataByid(It.IsAny<int>())).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<UserProfileDTO>(user)).Returns(userDTO);

            // Act
            IActionResult result = await _dataController.GetUserData();

            // Assert
            var test = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserProfileDTO>(test.Value);
        }

        [Fact]
        public async Task DataController_UserAdded_ReturnOk()
        {
            var userId = 1;
            var request = new RegisterUserRequest { Login = "Kactus", Email = "sasa@gmail", Password = "52" };
            var user = new User { Id = userId, Name = "Guts", Email = "sasa@gmail" };
            _mockMapper.Setup(m => m.Map<User>(request)).Returns(user);
            _mockDataRepository.Setup(x => x.AddUser(user));//Хотелось бы сделать нормальную проверку через ValueTask, но из-за махинаций с транзакциями внутри репозитория не выйдет

            IActionResult result = await _dataController.AddUser(request);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}