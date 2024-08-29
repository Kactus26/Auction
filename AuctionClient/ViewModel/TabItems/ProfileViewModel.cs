using AuctionClient.Data;
using AuctionClient.View;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Shapes;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class ProfileViewModel : ObservableObject
    {
        #region UserData
        [ObservableProperty]
        public string name = "";
        [ObservableProperty]
        public string surName = "";
        [ObservableProperty]
        public string email = "";
        [ObservableProperty]
        public bool emailWarningEnabled = true;
        [ObservableProperty]
        public string description = "";
        [ObservableProperty]
        public double balance;
        [ObservableProperty]
        public string userImagePath;
        #endregion
        private byte[] ImageToSend { get; set; }
        private string? ErrorMessage {  get; set; }

        private string emailInDb;//Needs to ckeck if email is updated for it's IsConfirmed status

        private const string pathToImages = "../../../Images/";

        private const string gatewayPort = "https://localhost:7002";

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

        private async Task<bool> Post<T>(T request, string methodName)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
                $"{gatewayPort}/api/Data/{methodName}", request);

            if (!response.IsSuccessStatusCode)
            {
                if (response.ReasonPhrase == "Unauthorized")
                    MessageBox.Show("Guest can't do this function");
                else
                {
                    ErrorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(ErrorMessage);
                }
                return false;
            }
            var test = await response.Content.ReadAsStringAsync();
            return true;
        }//Method useful only if no data comes as response

        [RelayCommand]
        public async Task UpdateUserData()
        {
            UserDataWithImageDTO newData = new UserDataWithImageDTO { ProfileData = new UserProfileDTO { Name = this.Name, Email = this.Email, Surname = this.SurName, Info = Description, Balance = this.Balance, IsEmailConfirmed = !this.EmailWarningEnabled} };

            if (emailInDb != newData.ProfileData.Email)
            {
                newData.ProfileData.IsEmailConfirmed = false;
                EmailWarningEnabled = true;
            }

            if (ImageToSend != null) 
                newData.Image = ImageToSend;

            if(await Post(newData, "UpdateUserData"))
                MessageBox.Show("Data successfully updated! Congrats!");
        }

        private async void GetUserData()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{gatewayPort}/api/Data/GetUserData");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ErrorMessage);
                return;
            }

            UserDataWithImageDTO user = JsonConvert.DeserializeObject<UserDataWithImageDTO>(await response.Content.ReadAsStringAsync());

            if (user != null)
            {
                Name = user.ProfileData.Name;
                SurName = user.ProfileData.Surname;
                Email = user.ProfileData.Email;
                Description = user.ProfileData.Info;
                Balance = user.ProfileData.Balance;
                EmailWarningEnabled = !user.ProfileData.IsEmailConfirmed;

                emailInDb = Email;

                if (user.Image != null)
                {
                    string relativePath = pathToImages + "ProfileImage.png";
                    string absolutePath = System.IO.Path.GetFullPath(relativePath);

                    using (var stream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
                    {
                        stream.Write(user.Image);
                    }

                    UserImagePath = absolutePath;
                }
            }
            else
                MessageBox.Show("User data not found");
        }

        [RelayCommand]
        public async Task UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                ImageToSend = File.ReadAllBytes(filePath);

                UserImagePath = filePath;
            }
        }

        public void ProfileForGuest()
        {
            Balance = 0;
            Name = "Guest";
            SurName = "Guestovich";
            Email = "guest.guestov@gmail.guest";
            Description = "I'm just guest";
            UserImagePath = pathToImages + "Guest.jpg";
            EmailWarningEnabled = false;
        }

        [RelayCommand]
        public async Task ConfirmEmail()
        {
            EmailDTO emailDTO = new EmailDTO { Email = Email };

            var response = await _httpClient.PostAsJsonAsync(
                $"{gatewayPort}/api/Identity/SendEmail", emailDTO);

            if (!response.IsSuccessStatusCode)
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            else
            {
                string ConfirmEmailPassword = await response.Content.ReadAsStringAsync();
                await OpenModalWindow(ConfirmEmailPassword);
            }
        }

        public async Task OpenModalWindow(string ConfirmEmailPassword)
        {
            ConfirmEmail modalWindow = new ConfirmEmail(ConfirmEmailPassword);

            bool? result = modalWindow.ShowDialog();

            if (result == true)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{gatewayPort}/api/Data/EmailIsConfirmed");
                if (response.IsSuccessStatusCode)
                {
                    EmailWarningEnabled = false;
                    emailInDb = this.Email;
                    MessageBox.Show($"Email is successfully confirmed");
                }
                else
                    MessageBox.Show($"{await response.Content.ReadAsStringAsync()}");
            }
            else
                MessageBox.Show("Email is not confirmed(");
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

            System.Windows.Application.Current.Windows[0].Close();
        }
    }
}
