using Auction.Tests.Data;
using AuctionIdentity.Models;
using CommonDTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Xunit;

namespace AuctionServer.Tests.IntegrationalTests.AuctionServerTests.Controllers
{
    public class DataControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DbContextOptions<DataContext> _options;

        public DataControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDbForTesting")
                .Options;
        }

        [Fact]
        public async Task DataController_CheckUserData_ReturnOk()
        {
            // Arrange
            var user = new User { Id = 1, Login = "Kactus", Email = "Test@test", Password = "Test"};
            var token = _jwtProvider.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("api/Data/GetUserData");

            // Assert
            response.EnsureSuccessStatusCode();
            UserProfileDTO result = JsonConvert.DeserializeObject<UserProfileDTO>(await response.Content.ReadAsStringAsync())!;
            Assert.IsType<UserProfileDTO>(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public void TestInMemoryDatabase()
        {
            // Arrange
            using (var context = new DataContext(_options))
            {
                var entity = new AuctionServer.Model.User { Name = "Test", Email = "asdadsda" };
                context.Users.Add(entity);
                context.SaveChanges();
            }

            // Act
            using (var context = new DataContext(_options))
            {
                var result = context.Users.FirstOrDefault();

                // Assert
                Assert.NotNull(result);
                // Add more assertions as needed
            }
        }
    }
}
