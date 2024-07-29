using AuctionIdentity.Models;
using CommonDTO;
using FluentAssertions;
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

        public static void ChangeProperties<T>(T target, T source)
        {
            var properties = typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite && p.Name != "Id");

            foreach(var property in properties)
            {
                var value = property.GetValue(source);
                property.SetValue(target, value);
            }
        }

        [Fact]
        public async Task DataController_GetUserData_ReturnOk()
        {
            // Arrange
            var user = new User { Id = 1, Login = "Kactus", Email = "Test@test", Password = "Test" };
            var token = _jwtProvider.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.GetAsync("api/Data/GetUserData");
            response.EnsureSuccessStatusCode();

            // Assert
            UserDataWithImageDTO result = JsonConvert.DeserializeObject<UserDataWithImageDTO>(await response.Content.ReadAsStringAsync())!;
            Assert.IsType<UserDataWithImageDTO>(result);
            Assert.Equal(user.Id, result.ProfileData.Id);
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

        [Fact]
        public async Task DataController_UpdateUserData_ReturnOk()
        {
            //Arrange
            Model.User userBeforeUpdate = await _dataContext.Users.AsNoTracking().FirstOrDefaultAsync();

            var user = new User { Id = userBeforeUpdate.Id, Login = "Kactus", Email = "Test@test", Password = "Test" }; //Id from here
            var token = _jwtProvider.GenerateToken(user);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            UserProfileDTO newData = new UserProfileDTO() { Name = "UpdatedKactus", Email = "UpdatedEmail", Info = "I'm updated...", Surname = "UpdatedSurname", ImageUrl = "sadasda", Balance = 0 };
            Model.User fakeUpdatedUser = new Model.User() { Id = userBeforeUpdate.Id, Name = "UpdatedKactus", Email = "UpdatedEmail", Info = "I'm updated...", Surname = "UpdatedSurname", ImageUrl = "sadasda", Balance = 0 };

            //Act
            var response = await _client.PostAsJsonAsync("api/Data/UpdateUserData", newData);
            response.EnsureSuccessStatusCode();

            Model.User updatedUser = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == userBeforeUpdate.Id);

            //Assert
            updatedUser.Should().BeEquivalentTo(fakeUpdatedUser);

            //Clear
            ChangeProperties(updatedUser, userBeforeUpdate);
            _dataContext.SaveChanges();
        }

    }
}
