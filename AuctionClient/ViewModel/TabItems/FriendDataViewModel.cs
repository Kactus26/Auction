using AuctionClient.Data;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http;
using AuctionServer.Model;
using Newtonsoft.Json;
using static AuctionClient.ViewModel.TabItems.FriendsViewModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class FriendDataViewModel : ObservableObject
    {
        #region UserData
        [ObservableProperty]
        public string name = "";
        [ObservableProperty]
        public string surName = "";
        [ObservableProperty]
        public string email = "";
        [ObservableProperty]
        public string description = "";
        [ObservableProperty]
        public byte[] image;
        [ObservableProperty]
        public bool isAddFriendEnabled = true;
        [ObservableProperty]
        public bool isRemoveFriendEnabled = false;
        [ObservableProperty]
        public bool isBlockUserEnabled = true;
        [ObservableProperty]
        public bool isUnblockUserEnabled = false;
        #endregion

        private readonly int friendId;
        private readonly int userId;
        private const string gatewayPort = "http://localhost:5175";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public FriendDataViewModel(UserDataWithImageDTO userData)
        {

            friendId = userData.ProfileData.Id;
            Name = userData.ProfileData.Name;
            SurName = userData.ProfileData.Surname;
            Email = userData.ProfileData.Email;
            Description = userData.ProfileData.Info;
            Image = userData.Image;

            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                UsersFriendshipStatus();
            } 
            else
            {
                isBlockUserEnabled = false;
                isAddFriendEnabled = false;
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(lu.JWTToken))
            {
                // Расшифровка JWT без проверки подписи
                var jwtToken = handler.ReadJwtToken(lu.JWTToken);

                // Доступ к claims (утверждениям)
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

                userId = System.Convert.ToInt32(userIdClaim.Value);
            }

            if (friendId == userId)
            {
                isBlockUserEnabled = false;
                isAddFriendEnabled = false;
            }
        }

        [RelayCommand]
        public async Task BlockUser()
        {
            MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to block this user? Blocked user won't be able to see your profile or your lots...",
                    "Confirm",
                    MessageBoxButton.YesNo
                );

            if (result == MessageBoxResult.Yes)
            {
                UserIdDTO friendToSendId = new() { Id = friendId };
                var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/BlockUser", friendToSendId);

                string responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Error in BlockUser method {responseContent}");
                    return;
                }
                else
                {
                    MessageBox.Show("User blocked!");
                    IsAddFriendEnabled = false;
                    IsRemoveFriendEnabled = false;
                    IsUnblockUserEnabled = true;
                    IsBlockUserEnabled = false;
                    return;
                }
            }
        }

        [RelayCommand]
        public async Task UnblockUser()
        {
            UserIdDTO friendToSendId = new() { Id = friendId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/UnblockUser", friendToSendId);

            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Error in UnblockUser method {responseContent}");
                return;
            }
            else
            {
                MessageBox.Show("User unblocked!");
                IsAddFriendEnabled = true;
                IsBlockUserEnabled = true;
                IsUnblockUserEnabled = false;
                return;
            }
        }

        [RelayCommand]
        public async Task AddFriend()
        {
            UserIdDTO friendToSendId = new() { Id = friendId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/AddFriend", friendToSendId);

            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Error in AddFriend method {responseContent}");
                return;
            } else if(responseContent == "Request send")
            {
                MessageBox.Show("Request send!");
                IsAddFriendEnabled = false;
                return;
            }

            Friendship friendship = JsonConvert.DeserializeObject<Friendship>(responseContent);

            IsAddFriendEnabled = false;

            if (friendship.Relations == FriendStatus.Send)
                MessageBox.Show("Friend request send!");
            else if (friendship.Relations == FriendStatus.Friend)
            {
                IsRemoveFriendEnabled = true;
                MessageBox.Show("You are now friends!");
            }
        }

        [RelayCommand]
        public async Task RemoveFriend()
        {
            UserIdDTO friendToSendId = new() { Id = friendId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/RemoveFriend", friendToSendId);

            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"Error in RemoveFriend method! {responseContent}");
            else
            {
                IsAddFriendEnabled = true;
                IsRemoveFriendEnabled=false;
                MessageBox.Show($"Friend deleted!");
            }
        }

        private async Task UsersFriendshipStatus()
        {
            UserIdDTO friendToSendId = new() { Id = friendId };

            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetUsersFriendshipStatus", friendToSendId);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Friendship friendship = JsonConvert.DeserializeObject<Friendship>(responseContent);
                if (friendship.Relations == FriendStatus.Friend)
                {
                    IsAddFriendEnabled = false;
                    IsRemoveFriendEnabled = true;
                } else if (friendship.FriendId == friendId && friendship.Relations == FriendStatus.Send)
                {
                    IsAddFriendEnabled = false;
                } else if (friendship.Relations == FriendStatus.Blocked && friendship.WhoBlockedId != friendId)
                {
                    IsAddFriendEnabled = false;
                    IsUnblockUserEnabled = true;
                    IsBlockUserEnabled = false;
                }
                    
            } else
                MessageBox.Show($"Error in UsersFriendshipStatus merhod: {responseContent}");

        }
    }
}
