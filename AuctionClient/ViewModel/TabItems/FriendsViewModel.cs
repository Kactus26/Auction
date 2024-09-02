using AuctionClient.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using AuctionServer.Model;
using CommunityToolkit.Mvvm.Input;
using CommonDTO;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class FriendsViewModel : ObservableObject
    {
        [ObservableProperty]
        public ICollection<UserDataWithImageDTO> friends;
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string findUser;

        ICollection<User>? UserFriends { get; set; }
        private const string gatewayPort = "https://localhost:7002";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public FriendsViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                GetUserFriends();
            }
            else
                Name = "Guest can't have any friends( But he can find other users";

        }

        private async void GetUserFriends()
        {
            var response = await _httpClient.GetAsync($"{gatewayPort}/api/Data/GetUserFriends");
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                Friends = JsonConvert.DeserializeObject<ICollection<UserDataWithImageDTO>>(responseContent);
            else
                MessageBox.Show($"{responseContent}");
        }

        /*[RelayCommand]
        public async Task FindUser()
        {
            var result = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/FindUser", FindUser);
        }*/
    }
}
