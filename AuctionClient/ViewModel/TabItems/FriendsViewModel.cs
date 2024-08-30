using AuctionClient.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class FriendsViewModel : ObservableObject
    {
        private const string gatewayPort = "https://localhost:7002";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public FriendsViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
            else
                MessageBox.Show("Guest doesn't have any friends(");

            GetUserFriends();
        }

        private async Task GetUserFriends()
        {
            var response = await _httpClient.GetAsync($"{gatewayPort}/api/Data/GetUserFriends");
        }
    }
}
