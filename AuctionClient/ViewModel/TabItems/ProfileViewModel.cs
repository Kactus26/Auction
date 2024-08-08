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
        public string description = "";
        [ObservableProperty]
        public double balance;
        [ObservableProperty]
        public string userImagePath;
        #endregion
        private byte[] ImageToSend { get; set; }
        private string? ErrorMessage {  get; set; }
        private string? ConfirmEmailPassword {  get; set; }

        private const string pathToImages = "../../../Images/";

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
                $"https://localhost:7002/api/Data/{methodName}", request);

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
            return true;
        }

        [RelayCommand]
        public async Task UpdateUserData()
        {
            if (Name.Length == 0 || SurName.Length == 0 || Email.Length == 0) {
                MessageBox.Show("Name, Surname and Email have to be greater than 1 symbol");
            }

            UserDataWithImageDTO newData = new UserDataWithImageDTO { ProfileData = new UserProfileDTO { Name = this.Name, Surname = this.SurName, Email = this.Email, Info = Description, Balance = this.Balance } };
            
            if(ImageToSend != null) 
                newData.Image = ImageToSend;

            if(await Post(newData, "UpdateUserData"))
                MessageBox.Show("Data successfully updated! Congrats!");
        }

        private async void GetUserData()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7002/api/Data/GetUserData");

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

                string relativePath = pathToImages + "ProfileImage.png";
                string absolutePath = System.IO.Path.GetFullPath(relativePath);

                using (var stream = new FileStream(absolutePath, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(user.Image);
                }

                UserImagePath = absolutePath;
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
        }

        [RelayCommand]
        public async Task ConfirmEmail()
        {
            EmailDTO emailDTO = new EmailDTO { Email = Email };
            var response = await _httpClient.PostAsJsonAsync(
                $"https://localhost:7002/api/Identity/SendEmail", emailDTO);
            if (!response.IsSuccessStatusCode)
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            else
                ConfirmEmailPassword = await response.Content.ReadAsStringAsync();
        }

        public void OpenModalWindow()
        {
            
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
