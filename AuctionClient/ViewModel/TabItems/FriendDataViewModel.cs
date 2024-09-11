using AuctionClient.Data;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http;
using AuctionServer.Model;
using Newtonsoft.Json;
using static AuctionClient.ViewModel.TabItems.FriendsViewModel;

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
        #endregion

        private readonly int userId;
        private const string gatewayPort = "https://localhost:7002";
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
                isAddFriendEnabled = false;
        }

        private async Task UsersFriendshipStatus()
        {
            UserIdDTO userToFindStatus = new UserIdDTO { Id = userId };

            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetUsersFriendshipStatus", userToFindStatus);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                if (responseContent == "0" || responseContent == "2")
                    IsAddFriendEnabled = false;
            }
        }
    }
}
