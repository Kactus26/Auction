using AuctionClient.Data;
using AuctionClient.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        public string userImage = "../../Images/Guest.jpg";

        private readonly HttpClient _httpClient;
        ApplicationContext db = new ApplicationContext();

        public ProfileViewModel()
        {
            _httpClient = new HttpClient();
            LoggedUser lu = db.Find<LoggedUser>(1)!;
            if (lu != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", lu.JWTToken);
            else
                ProfileForGuest();

        }

        public void ProfileForGuest()
        {
            UserImage = "../../Images/Guest.jpg";
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
