using AuctionClient.Data;
using AuctionServer.Model;
using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class LotConstructorViewModel : ObservableObject
    {
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public double startPrice;
        [ObservableProperty]
        public string description;
        [ObservableProperty]
        public string userImagePath;


        private byte[] ImageToSend { get; set; }
        private const string pathToImages = "../../../Images/";
        private const string gatewayPort = "http://localhost:5175";
        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();


        public LotConstructorViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
            }
        }

        [RelayCommand]
        public async Task CreateLot()
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description) || double.IsNegative(StartPrice) || ImageToSend == null)
            {
                MessageBox.Show("All fields must be filled in, the image is uploaded and the price is positive");
                return;
            }

            CreateLotWithImageDTO lotWithImageDTO = new() { Name = Name, Description = Description, StartPrice = StartPrice, Image = ImageToSend };
            var response = await _httpClient.PostAsJsonAsync($"{gatewayPort}/api/Data/CreateLot", lotWithImageDTO);

            string result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                MessageBox.Show($"Something went wrong in CreateLot {result}");

            MessageBox.Show($"{result}");
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
    }
}
