using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        private HttpClient _httpClient;

        private const string gatewayPort = "https://localhost:7002";

        public PasswordRecoveryViewModel()
        {
            _httpClient = new HttpClient();
        }

        [RelayCommand]
        public async Task SendCode()
        {
            try
            {
                string userId = await Post(Login, "Identity", "UserIdPasswordRecovery");

                string isEmailConfirmed = await Post(userId, "Data", "IsEmailComfirmed");

                string code = await Post(userId, "Identity", "SendCodePasswordRecovery");

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
