using AuctionServer.Tests.IntegrationalTests;
using CommonDTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;

namespace Auction.Tests.IntegrationalTests.AuctionIdentityTests.Controllers
{
    public class UserControllerTests : IntegrationTestBase, IClassFixture<WebApplicationFactory<IdentityProgram>>
    {
        private readonly HttpClient _client;
        public UserControllerTests(WebApplicationFactory<IdentityProgram> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UserController_SendEmail_ReturnOk()
        {
            RegisterUserRequest request = new RegisterUserRequest { Email = "sasha.baginsky@gmail.com", Login = "123", Password = "123" };

            var response = await _client.PostAsJsonAsync("api/User/SendEmail", request);

            string result = await response.Content.ReadAsStringAsync();

            Assert.NotNull(result);
            Assert.Equal("Email Send Succsessfully", result);
        }
    }
}
