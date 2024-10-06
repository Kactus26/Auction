using AuctionClient.Data;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class LotViewModel : ObservableObject
    {
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string description;
        [ObservableProperty]
        public string startedAt;
        [ObservableProperty]
        public byte[] image;
        [ObservableProperty]
        public double balance;
        [ObservableProperty]
        public double userOffer;
        [ObservableProperty]
        public List<OffersDTO> offers = new List<OffersDTO>();
        
        [ObservableProperty]
        public ObservableCollection<UserDataWithImageDTO> userData = new ObservableCollection<UserDataWithImageDTO>();

        private int lotId;
        private const string gatewayPort = "http://localhost:5175";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public LotViewModel(LotWithImageDTO lot)
        {
            _httpClient = new HttpClient();

            lotId = lot.LotInfo.Id;
            Name = lot.LotInfo.Name;
            Description= lot.LotInfo.Description;
            StartedAt = "Started at: " + lot.LotInfo.DateTime;
            Image = lot.Image;

            GetLotSellerInfo(lot.LotInfo.Id);

            GetLotOffersInfo(lot.LotInfo.Id);

            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                GetUserBalance();
            }
        }

        [RelayCommand]
        public async Task SendOffer()
        {
            OfferPrice offerPrice = new OfferPrice() { LotId = lotId, Price = UserOffer };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/SendOffer", offerPrice);

            MessageBox.Show(await response.Content.ReadAsStringAsync());
        }

        private async Task GetUserBalance()
        {
            var response = await _httpClient.GetAsync($"{gatewayPort}/api/Data/GetUserBalance");

            if(!response.IsSuccessStatusCode)
                return;

            Balance = await response.Content.ReadFromJsonAsync<double>();
        }

        private async Task GetLotSellerInfo(int lotId)
        {
            UserIdDTO lotIdDTO = new () { Id = lotId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetLotSellerInfo", lotIdDTO);
            string result = await response.Content.ReadAsStringAsync();

            UserDataWithImageDTO userDTO = JsonConvert.DeserializeObject<UserDataWithImageDTO>(result)!;
            UserData.Add(userDTO);
        }

        private async Task GetLotOffersInfo(int lotId)
        {
            UserIdDTO lotIdDTO = new() { Id = lotId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetLotOffersInfo", lotIdDTO);
            string result = await response.Content.ReadAsStringAsync();

            ICollection<OffersDTO> offersDTO = JsonConvert.DeserializeObject<List<OffersDTO>>(result)!;
            Offers = offersDTO.ToList();
        }
    }
}
