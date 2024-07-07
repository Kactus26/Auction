using AuctionIdentity.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using Xunit;

namespace AuctionServer.Tests.IntegrationalTests.AuctionServerTests.Controllers
{
    public class DataController : IntegrationTestBase, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DataController(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Test()
        {
            // Arrange
            var user = new User { Id = 1, Login = "Kactus", Email = "Test@test", Password = "Test"}; // Создайте объект User
            var token = _jwtProvider.GenerateToken(user); // Генерация токена с использованием JWTProvider

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("api/Data/GetUserData");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("expectedValue", responseString);
        }
    }
}
//_httpClient.PostAsJsonAsync($"https://localhost:7002/api/{controllerName}/{methodName}", request);