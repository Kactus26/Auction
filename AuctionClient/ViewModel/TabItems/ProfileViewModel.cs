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
        public string? userImagePath;
        private bool IsGuest { get; set; } = false;

        /*        private ByteArrayContent? Image {  get; set; }
        */
        #endregion
        private string? ErrorMessage {  get; set; }

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
                ErrorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ErrorMessage);
                return false;
            }
            
            return true;
        }

        [RelayCommand]
        public async Task UpdateUserData()//Add User Image
        {
            if (IsGuest) {
                MessageBox.Show("Guest can't update his profile");
                return;
            } else if (Name.Length == 0 || SurName.Length == 0 || Email.Length == 0)
            {
                MessageBox.Show("Name, Surname and Email have to be greater than 1 symbol");
            }

            ChangedDataDTO newData = new ChangedDataDTO() { Name = this.Name, Surname = this.SurName, Email = this.Email, Info = Description, Balance = this.Balance};
            await Post(newData, "UpdateUserData");
        }

        private async void GetUserData()//Check User Image 
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7002/api/Data/GetUserData");

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = await response.Content.ReadAsStringAsync();
                MessageBox.Show(ErrorMessage);
                return;
            }

            UserProfileDTO userData = JsonConvert.DeserializeObject<UserProfileDTO>(await response.Content.ReadAsStringAsync())!;

            Name = userData.Name;
            SurName = userData.Surname;
            Email = userData.Email;
            Description = userData.Info;
            Balance = userData.Balance;
            UserImagePath = userData.ImageUrl;
        }

        [RelayCommand]
        public void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var fileBytes = File.ReadAllBytes(filePath);

                UserImagePath = filePath;

                /*var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                var formData = new MultipartFormDataContent();
                formData.Add(fileContent, "file", Path.GetFileName(filePath));

                var response = await _httpClient.PostAsync("https://yourapiurl/api/upload/upload", formData);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);*/
            }
        }

        public void ProfileForGuest()
        {
            Balance = 0;
            Name = "Guest";
            SurName = "Guestovich";
            Email = "guest.guestov@gmail.guest";
            Description = "I'm just guest";
            UserImagePath = "../../Images/Guest.jpg";
            IsGuest = true;
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
