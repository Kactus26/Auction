using AuctionClient.Data;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
        public List<OffersDTO>? offers = new List<OffersDTO>();
        [ObservableProperty]
        public ObservableCollection<UserDataWithImageDTO> ownerData = new ObservableCollection<UserDataWithImageDTO>();

        private bool _isUserOwner;
        public bool IsUserOwner//reverse value for button visability
        {
            get => _isUserOwner;
            set
            {
                _isUserOwner = !value;
                OnPropertyChanged(nameof(_isUserOwner));
            }
        }

        [ObservableProperty]
        private bool isLotClosedButtonShows = false;

        private bool isLotClosed;
        private int lotId;
        private bool isUserEmailConfirmed;
        private const string gatewayPort = "http://localhost:5175";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public LotViewModel()
        {
            _httpClient = new HttpClient();
        }

        public async Task InitializeAync(LotWithImageDTO lot)
        {
            lotId = lot.LotInfo.Id;

            StartedAt = "Started at: " + lot.LotInfo.DateTime;
            Image = lot.Image;

            await GetLotSellerInfo(lot.LotInfo.Id);

            await GetLotAndOffersInfo(lot.LotInfo.Id);

            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                await GetUserBalanceAndEmail();

                if (!IsUserOwner && !isLotClosed)
                    IsLotClosedButtonShows = true;
            }
        }

        [RelayCommand]
        public async Task CloseLot()
        {
            MessageBoxResult result = MessageBox.Show(
                    "Are you sure? There is no going back...",
                    "Confirm",
                    MessageBoxButton.YesNo
                );

            if (result == MessageBoxResult.No)
                return;

            UserIdDTO lotIdDTO = new UserIdDTO() { Id = lotId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/CloseLot", lotIdDTO);

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Something went wrong in CloseLot method");
                return;
            }

            IsLotClosedButtonShows = false;
            MessageBox.Show(await response.Content.ReadAsStringAsync());
        }

        [RelayCommand]
        public async Task SendOffer()
        {
            if (isUserEmailConfirmed != true)
            {
                MessageBox.Show("You need to confirm your email for this!", "Denied");
                return;
            }
            else if (UserOffer == 0)
            {
                MessageBox.Show("You need to have some balance for this!", "Denied");
                return;
            }

            OfferPrice offerPrice = new OfferPrice() { LotId = lotId, Price = UserOffer };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/SendOffer", offerPrice);

            MessageBox.Show(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode)
                return;

            await GetLotAndOffersInfo(lotId);
        }

        private async Task GetUserBalanceAndEmail()
        {
            var response = await _httpClient.GetAsync($"{gatewayPort}/api/Data/GetUserBalanceAndEmail");

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Something went wrong with request in GetUserBalanceAndEmail");
                return;
            }

            var userDTO = JsonConvert.DeserializeObject<UserBalanceAndEmailDTO>(await response.Content.ReadAsStringAsync());

            if (userDTO == null) 
            { 
                MessageBox.Show("Something went wrong with deserialization in GetUserBalanceAndEmail");
                return;
            }

            Balance = userDTO.Balance;
            isUserEmailConfirmed = userDTO.IsEmailConfirmed;

            if (userDTO.UserId == OwnerData.First().ProfileData.Id)
                IsUserOwner = true;
            else
                IsUserOwner = false;

        }

        private async Task GetLotSellerInfo(int lotId)
        {
            UserIdDTO lotIdDTO = new () { Id = lotId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetLotSellerInfo", lotIdDTO);
            string result = await response.Content.ReadAsStringAsync();

            UserDataWithImageDTO userDTO = JsonConvert.DeserializeObject<UserDataWithImageDTO>(result)!;
            OwnerData.Add(userDTO);
        }

        private async Task GetLotAndOffersInfo(int lotId)
        {
            UserIdDTO lotIdDTO = new() { Id = lotId };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/GetLotAndOffersInfo", lotIdDTO);
            string result = await response.Content.ReadAsStringAsync();

            LotWithOfferDTO lotWithOffersDTO = JsonConvert.DeserializeObject<LotWithOfferDTO>(result)!;

            if (!Offers.IsNullOrEmpty())
                Offers.Clear();

            Offers = lotWithOffersDTO.Offers.ToList();

            Name = lotWithOffersDTO.LotInfo.Name;
            Description = lotWithOffersDTO.LotInfo.Description;
            isLotClosed = lotWithOffersDTO.LotInfo.IsClosed;
        }
    }
}
