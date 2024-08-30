using AuctionIdentity.Models;
using AuctionServer.Tests.IntegrationalTests;
using CommonDTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Auction.Tests.IntegrationalTests.AuctionServerTests.Controllers
{
    public class FriendsControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<ServerProgram>>
    {
        private readonly HttpClient _client;

        public FriendsControllerTests(WebApplicationFactory<ServerProgram> factory)
        {
            _client = factory.CreateClient();
        }

        private static void ChangeProperties<T>(T target, T source)
        {
            var properties = typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite && p.Name != "Id");

            foreach (var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }

        [Fact]
        public async Task FriendsController_GetUserFriends_ReturnOk()
        {
            // Arrange
            var user = new User { Id = 3, Login = "Kactus", Password = "Test" };
            var token = _jwtProvider.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("api/Friends/GetUserFriends");
            response.EnsureSuccessStatusCode();
            ICollection<AuctionServer.Model.User> result = 
                JsonConvert.DeserializeObject<ICollection<AuctionServer.Model.User>>(await response.Content.ReadAsStringAsync())!;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result.FirstOrDefault());
        }
    }
}
