using AuctionClient.Data;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using static AuctionClient.ViewModel.TabItems.FriendsViewModel;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class AllLotsViewModel : ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<LotWithImageDTO> lots1 = new ObservableCollection<LotWithImageDTO>();
        [ObservableProperty]
        public ObservableCollection<LotWithImageDTO> lots2 = new ObservableCollection<LotWithImageDTO>();
        [ObservableProperty]
        public string? name;
        [ObservableProperty]
        public int currentPage = 1;
        [ObservableProperty]
        public bool isPreviousPageEnabled = false;

        const int pageSize = 7;
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();
        private const string gatewayPort = "http://localhost:5175";
        private typesOfLot typeOfLot = typesOfLot.Normal;
        private enum typesOfLot
        {
            Normal,
            Search
        }

        public AllLotsViewModel()
        {
            _httpClient = new HttpClient();
            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
            }
            await GetLots();
        }


        [RelayCommand]
        public async Task SearchButton()
        {
            CurrentPage = 1;
            await Search();
        }

        private async Task Search()
        {
            PaginationLotSearchDTO lotSearchDTO = new () { Name = Name, CurrentPage = CurrentPage, PageSize = pageSize };

            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/FindLot", lotSearchDTO);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"{responseContent}");
                return;
            }

            List<LotWithImageDTO>? all = JsonConvert.DeserializeObject<List<LotWithImageDTO>>(responseContent);
            typeOfLot = typesOfLot.Search;
            LotAllocation(all);
        }

        private async Task GetLots()
        {
            PaginationDTO paginationDTO = new() { CurrentPage = CurrentPage, PageSize = pageSize };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetLots", paginationDTO);

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Something went wrong in GetLots {result}");
                return;
            }

            List<LotWithImageDTO> lotWithImageDTO = JsonConvert.DeserializeObject<List<LotWithImageDTO>>(result);

            LotAllocation(lotWithImageDTO);
        }

        private void LotAllocation(List<LotWithImageDTO> all)
        {
            Lots1.Clear();
            Lots2.Clear();

            int index = 0;
            while (index < all.Count)
            {
                if (index <= 3)
                {
                    Lots1.Add(all[index]);
                }
                else
                    Lots2.Add(all[index]);
                index++;
            }
        }

        [RelayCommand]
        public async Task NextPage()
        {
            CurrentPage++;
            IsPreviousPageEnabled = true;
            if (typeOfLot == typesOfLot.Normal)
                await GetLots();
            else
                await Search();
        }

        [RelayCommand]
        public async Task PreviousPage()
        {
            if (CurrentPage == 2)
                IsPreviousPageEnabled = false;

            CurrentPage--;

            if (typeOfLot == typesOfLot.Normal)
                await GetLots();
            else
                await Search();
        }
    }
}
