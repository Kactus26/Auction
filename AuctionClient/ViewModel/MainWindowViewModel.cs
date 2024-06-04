using AuctionClient.Data;
using AuctionClient.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionClient.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        ApplicationContext db = new ApplicationContext();

        [RelayCommand]
        public void ExitFromAccount()
        {
            LoggedUser loggedUser = db.Find<LoggedUser>(1)!;
            if(loggedUser != null) { 
                db.Remove(loggedUser);
                db.SaveChanges();
            }

            Registration regWindow = new Registration();
            regWindow.Show();
            System.Windows.Application.Current.Windows[0].Close();
        }
    }
}
