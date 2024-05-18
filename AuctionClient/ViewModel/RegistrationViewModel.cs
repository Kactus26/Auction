using AuctionClient.View;
using Common.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlzEx.Standard;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;

namespace AuctionClient.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        public RegistrationViewModel()
        {
            _httpClient = new HttpClient();
        }

        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        private string password = "";
        [ObservableProperty]
        private string confPassword = "";

        [RelayCommand]
        public async Task Registration()
        {
            RegisterUserRequest test = new RegisterUserRequest() { UserName = "lol", Email = "lol", Password = "123" };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
               $"https://localhost:7002/api/Identity/Registration", test);

            response.EnsureSuccessStatusCode();
        }
    }
}
