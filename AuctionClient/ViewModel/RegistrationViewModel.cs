﻿using AuctionClient.View;
using Common.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

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
        private string login = "";
        [ObservableProperty]
        private string email = "sasha.baginsky@gmail.com";
        [ObservableProperty]
        private string password = "";
        [ObservableProperty]
        private string confPassword = "";
        [ObservableProperty]
        private string errorMessage = "";

        [RelayCommand]
        public async Task Registration()
        {
            if (Login.Length < 5)
            {
                ErrorMessage = "Login length must be higher than 4";
                return;
            }
            else if (Email.Length < 5) //Сделать регулярное выражение
            {
                ErrorMessage = "Mail length must be higher than 4";
                return;
            }
            else if (Password.Length < 5)
            {
                ErrorMessage = "Password length must be higher than 4";
                return;
            }
            else if (Password != ConfPassword)
            {
                ErrorMessage = "Passwords doesn't match";
                return;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            if (!match.Success)
            {
                ErrorMessage = "It's not an email";
                return;
            }

            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = Login, Email = Email, Password = Password };

            if (!await Post(registerUserRequest, "Registration"))
                return;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            System.Windows.Application.Current.Windows[0].Close();
        }

        [RelayCommand]
        public async Task Authorization()
        {
            
        }

        private async Task<bool> Post<T>(T request, string methodName)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"https://localhost:7002/api/Identity/{methodName}", request);
            if(!response.IsSuccessStatusCode && methodName == "Registration")
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                return false;
            }
            return true;
        }
    }
}
