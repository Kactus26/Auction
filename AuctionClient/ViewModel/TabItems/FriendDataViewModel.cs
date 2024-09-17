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
        #endregion

        private readonly int userId;
        private const string gatewayPort = "http://localhost:5175";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public FriendDataViewModel(UserDataWithImageDTO userData)
        {
            userId = userData.ProfileData.Id;
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
                isAddFriendEnabled = false;
                isRemoveFriendEnabled = false;
            }
        }

        [RelayCommand]
        public async Task AddFriend()
        {
            UserIdDTO friendId = new() { Id = userId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/AddFriend", friendId);

            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Error in AddFriend merhod {responseContent}");
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

        private async Task UsersFriendshipStatus()
        {
            UserIdDTO friendId = new() { Id = userId };

            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetUsersFriendshipStatus", friendId);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                Friendship friendship = JsonConvert.DeserializeObject<Friendship>(responseContent);
                if (friendship.Relations == FriendStatus.Friend || friendship.Relations == FriendStatus.Blocked)
                {
                    IsAddFriendEnabled = false;
                    IsRemoveFriendEnabled = true;
                } if (friendship.FriendId == userId && friendship.Relations == FriendStatus.Send)
                {
                    IsAddFriendEnabled = false;
                }
                    
            } else
                MessageBox.Show($"Error in UsersFriendshipStatus merhod: {responseContent}");

        }
    }
}
