using AuctionClient.Data;
using AuctionClient.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        public string name = "";
        [ObservableProperty]
        public string surName = "";
        [ObservableProperty]
        public string email = "";
        [ObservableProperty]
        public string description = "";
        [ObservableProperty]
        public double balance;
        [ObservableProperty]
        public string userImage;
        private string ErrorMessage {  get; set; }

        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public ProfileViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
                GetUserData();
            }
            else
                ProfileForGuest();

        }

        private async void GetUserData()
        {
            await Get("GetUserData");
        }

        private async Task<bool> Get(string methodName)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                $"https://localhost:7002/api/Data/{methodName}");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ErrorMessage);
                return false;
            }

            var test = await response.Content.ReadAsStringAsync();
            return true;
        }

        private async Task<bool> Post<T>(T request, string methodName)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"https://localhost:7002/api/Data/{methodName}", request);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ErrorMessage);
                return false;
            }
            
            return true;
        }

        public void ProfileForGuest()
        {
            Balance = 0;
            Name = "Guest";
            SurName = "Guestovich";
            Email = "guest.guestov@gmail.guest";
            Description = "I'm just guest";
            UserImage = "../../Images/Guest.jpg";
        }

        [RelayCommand]
        public void ExitFromAccount()
        {
            LoggedUser loggedUser = db.Find<LoggedUser>(1)!;
            if (loggedUser != null)
            {
                db.Remove(loggedUser);
                db.SaveChanges();
            }

            Registration regWindow = new Registration();
            regWindow.Show();
            System.Windows.Application.Current.Windows[0].Close();
        }
    }
}
