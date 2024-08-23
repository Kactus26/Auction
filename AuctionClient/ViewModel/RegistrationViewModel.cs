using AuctionClient.Data;
using AuctionClient.View;
using AuctionClient.View.EmailPages;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Windows;
using static System.Net.WebRequestMethods;

namespace AuctionClient.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private const string gatewayPort = "https://localhost:7002";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public RegistrationViewModel()
        {
            db.Database.EnsureCreated();
            _httpClient = new HttpClient();
            CheckToken();
        }

        public async void CheckToken()
        {
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);

                if (await Post(1, "Identity", "TestAuthGateway"))
                    ChangeWindow();
                else
                {
                    db.Remove(lu);
                    db.SaveChanges();
                }
                    
            }
        }

        #region Properties
        [ObservableProperty]
        private string login = "";
        [ObservableProperty]
        private string email = "sasha.baginsky@gmail.com";
        [ObservableProperty]
        private string password = "";
        [ObservableProperty]
        private string passwordReg = "";
        [ObservableProperty]
        private string confPassword = "";
        [ObservableProperty]
        private string errorMessage = "";
        [ObservableProperty]
        private string errorMessageReg = "";
        #endregion

        [RelayCommand]
        public void OpenModalWindow()
        {
            PasswordRecovery modalWindow = new();
            modalWindow.ShowDialog();
        }

        [RelayCommand]
        public async Task Registration()
        {
            if (Login.Length < 5)
            {
                ErrorMessageReg = "Login length must be higher than 4";
                return;
            }
            else if (Email.Length < 5)
            {
                ErrorMessageReg = "Mail length must be higher than 4";
                return;
            }
            else if (PasswordReg.Length < 5)
            {
                ErrorMessageReg = "Password length must be higher than 4";
                return;
            }
            else if (PasswordReg != ConfPassword)
            {
                ErrorMessageReg = "Passwords don't match";
                return;
            }

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            if (!match.Success)
            {
                ErrorMessageReg = "It's not an email";
                return;
            }

            RegisterUserRequest registerUserRequest = new RegisterUserRequest() { Login = Login, Email = Email, Password = PasswordReg };

            if (!await Post(registerUserRequest, "Identity", "Registration"))
                return;
            else
            {
                LoggedUser lu = db.Find<LoggedUser>(1)!;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);

                await Post(registerUserRequest, "Data", "AddUser");
            }

            ChangeWindow();
        }

        [RelayCommand]
        public async Task Authorization()
        {
            AuthUserRequest authUserRequest = new AuthUserRequest() { Login = Login, Password = Password };

            if (!await Post(authUserRequest, "Identity", "Authorization"))
                return;

            ChangeWindow();
        }

        private async Task<bool> Post<T>(T request, string controllerName, string methodName)
         {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"{gatewayPort}/api/{controllerName}/{methodName}", request);

            if (!response.IsSuccessStatusCode && methodName == "Registration")
            {
                ErrorMessageReg = await response.Content.ReadAsStringAsync();
                return false;
            }
            else if (!response.IsSuccessStatusCode && methodName == "Authorization")
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                return false;
            } else if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                ErrorMessageReg = await response.Content.ReadAsStringAsync();
                return false;
            }

            if (methodName == "Registration" || methodName == "Authorization")
            {
                string token = await response.Content.ReadAsStringAsync();
                LoggedUser user = new LoggedUser { JWTToken = token };
                db.Add(user);
                db.SaveChanges();
            }

            return true;
        }

        [RelayCommand]
        private void ChangeWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            System.Windows.Application.Current.Windows[0].Close();
        }
    }
}
