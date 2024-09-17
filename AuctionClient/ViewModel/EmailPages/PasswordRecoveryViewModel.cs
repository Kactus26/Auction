using AuctionClient.Interfaces;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Windows;

namespace AuctionClient.ViewModel.EmailPagesViewModel
{
    public partial class PasswordRecoveryViewModel : ObservableObject
    {
        #region properties

        [ObservableProperty]
        public string login = string.Empty;
        [ObservableProperty]
        public string code = string.Empty;
        [ObservableProperty]
        public string password = string.Empty;
        [ObservableProperty]
        public string confirmPassword = string.Empty;

        #endregion

        private string EmailCode { get; set; }
        private int UserId { get; set; }

        private HttpClient _httpClient;

        private const string gatewayPort = "http://localhost:5175";

        public PasswordRecoveryViewModel()
        {
            _httpClient = new HttpClient();
        }

        [RelayCommand]
        public async Task ChangePassword()
        {
            if(Code != EmailCode)
            {
                MessageBox.Show("Code is incorrect");
                return;
            } 
            else if(Password.Length < 4)
            {
                MessageBox.Show("Password must be 4 symbols or more");
                return;
            }
            else if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords are not similar");
                return;
            } 

            try
            {
                ChangeUserPasswordDTO passwordDTO = new() { Id = UserId, Password = this.Password };

                string result = await Post(passwordDTO, "Identity", "NewPassword");
                MessageBox.Show($"{result}. You may close this window)");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        [RelayCommand]
        public async Task SendCode()
        {
            try
            {
                LoginDTO login = new() { Login = this.Login };

                UserId = System.Convert.ToInt32(await Post(login, "Identity", "UserIdPasswordRecovery"));

                UserIdDTO userIdDTO = new() { Id = UserId };

                string email = await Post(userIdDTO, "Data", "IsEmailConfirmed");

                EmailDTO emailDTO = new() { Email = email };

                EmailCode = await Post(emailDTO, "Identity", "SendEmail");

                MessageBox.Show($"Code was send to your email - {email}");
            }

            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private async Task<string> Post<T>(T data, string controllerName, string methodName)
        {

            var result = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/{controllerName}/{methodName}", data);

            string resultMessage = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode != true)
                throw new ArgumentException(resultMessage);

            return resultMessage;

        }

    }
}
