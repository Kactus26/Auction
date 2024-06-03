using AuctionClient.Data;
using AuctionClient.View;
using Common.DTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace AuctionClient.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public RegistrationViewModel()
        {
            _httpClient = new HttpClient();
/*            LoggedUser loggedUser = db.Find<LoggedUser>(1)!;
            if (loggedUser != null)
                ChangeWindow();*/
        }

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

        [RelayCommand]
        public async Task Registration()
        {
            if (Login.Length < 5)
            {
                ErrorMessageReg = "Login length must be higher than 4";
                return;
            }
            else if (Email.Length < 5) //Сделать регулярное выражение
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
                ErrorMessageReg = "Passwords doesn't match";
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

            if (!await Post(registerUserRequest, "Registration"))
                return;

            ChangeWindow();
        }

        [RelayCommand]
        public async Task Authorization()
        {
            LoggedUser lg = db.Find<LoggedUser>(1)!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lg.JWTToken);

            await Post(1, "TestAuthGateway");

/*            AuthUserRequest authUserRequest = new AuthUserRequest() { Login = Login, Password = Password };

            if (!await Post(authUserRequest, "Authorization"))
                return;

            ChangeWindow();*/
        }

        private async Task<bool> Post<T>(T request, string methodName)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"https://localhost:7002/api/Identity/{methodName}", request);

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
                return false;
            }
                
            string token = await response.Content.ReadAsStringAsync();
            LoggedUser user = new LoggedUser { JWTToken = token };
            db.Add(user);
            db.SaveChanges();

            return true;
        }

        private void ChangeWindow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            System.Windows.Application.Current.Windows[0].Close();
        }
    }
}
