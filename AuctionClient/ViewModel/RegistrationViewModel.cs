using AuctionClient.View;
using Common.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlzEx.Standard;
using Polly;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

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
        private string email = "";
        [ObservableProperty]
        private string password = "";
        [ObservableProperty]
        private string confPassword = "";
        [ObservableProperty]
        private string errorMessage = "";

        [RelayCommand]
        public async Task Registration()
        {
/*            if (Login.Length < 5)
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
            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = Login, Email = Email, Password = Password };*/

            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = "Kactus", Email = "lol", Password = "123" };

            await Post(registerUserRequest, "Registration");
        }

        private async Task Post<T>(T request, string methodName)
        {
            switch (methodName)
            {
                case "Registration":
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                        $"https://localhost:7002/api/Identity/Registration", request);
                    if(!response.IsSuccessStatusCode)
                    {
                        ErrorMessage = await response.Content.ReadAsStringAsync();
                    }
                    break;
/*                case "CheckUser":
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                        $"https://localhost:7002/api/Identity/CheckUser", request);
                    break;*/
            }


            
        }

    }
}
