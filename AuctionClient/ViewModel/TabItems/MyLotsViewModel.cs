using AuctionClient.Data;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using static AuctionClient.ViewModel.TabItems.FriendsViewModel;
using AuctionClient.View.TabItems;
using AuctionServer.Model;
using CommunityToolkit.Mvvm.Input;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class MyLotsViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<LotWithImageDTO> lots1 = new ObservableCollection<LotWithImageDTO>();
        [ObservableProperty]
        public ObservableCollection<LotWithImageDTO> lots2 = new ObservableCollection<LotWithImageDTO>();
        [ObservableProperty]
        public int currentPage = 1;
        [ObservableProperty]
        public bool isPreviousPageEnabled = false;


        const int pageSize = 7;
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();
        private const string gatewayPort = "http://localhost:5175";

        public MyLotsViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                GetUserLots();
            }
        }

        private async Task GetUserLots()
        {
            PaginationDTO paginationDTO = new() { CurrentPage = this.CurrentPage, PageSize = pageSize };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetUserLots", paginationDTO);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                List<LotWithImageDTO>? all = JsonConvert.DeserializeObject<List<LotWithImageDTO>>(responseContent);
                UsersAllocation(all);
            }
            else
                MessageBox.Show($"{responseContent}");
        }

        private void UsersAllocation(List<LotWithImageDTO> all)
        {
            Lots1.Clear();
            Lots2.Clear();

            int index = 0;
            while (index < all.Count)
            {
                if (index <= 3)
                    Lots1.Add(all[index]);
                else
                    Lots2.Add(all[index]);
                index++;
            }
        }

        [RelayCommand]
        public async Task AddLot()
        {
            
        }

        [RelayCommand]
        public async Task NextPage()
        {
            CurrentPage++;
            IsPreviousPageEnabled = true;
            GetUserLots();
        }

        [RelayCommand]
        public async Task PreviousPage()
        {
            if (CurrentPage == 2)
                IsPreviousPageEnabled = false;

            if (CurrentPage == 1)
            {
                return;
            }
            else
                CurrentPage--;



            GetUserLots();
        }
    }
}
