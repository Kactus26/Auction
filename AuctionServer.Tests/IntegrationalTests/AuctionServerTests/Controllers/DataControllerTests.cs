using Auction.Tests.Data;
using AuctionIdentity.Models;
using CommonDTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace AuctionServer.Tests.IntegrationalTests.AuctionServerTests.Controllers
{
    public class DataControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DataControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DataController_CheckUserData_ReturnOk()
        {
            // Arrange
            var user = new User { Id = 1, Login = "Kactus", Email = "Test@test", Password = "Test" };
            var token = _jwtProvider.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("api/Data/GetUserData");
            response.EnsureSuccessStatusCode();

            // Assert
            response.EnsureSuccessStatusCode();
            UserProfileDTO result = JsonConvert.DeserializeObject<UserProfileDTO>(await response.Content.ReadAsStringAsync())!;
            Assert.IsType<UserProfileDTO>(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task DataController_AddUser_ReturnOk()
        {
            // Arrange
            RegisterUserRequest request = new RegisterUserRequest() { Login = "Kactus", Email = "Test@test", Password = "Test" }; //Email from here
            var user = new User { Id = 10000, Login = "Kactus", Email = "Test@test", Password = "Test" }; //Id from here
            var token = _jwtProvider.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //Act
            var response = await _client.PostAsJsonAsync("api/Data/AddUser", request);
            response.EnsureSuccessStatusCode();

            var userAdded = await _dataContext.Users.Where(x => x.Id == user.Id && x.Email == request.Email).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(userAdded);

            //Clear
            _dataContext.Users.Remove(userAdded);
            _dataContext.SaveChanges();
        }
    }
}
