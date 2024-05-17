using AuctionClient.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlzEx.Standard;
using System.Windows;

namespace AuctionClient.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = "";
        [ObservableProperty]
        private string password = "";
        [ObservableProperty]
        private string confPassword = "";

        [RelayCommand]
        public void Test()
        {
            Name = Password;
            /*MainWindow window = new MainWindow();
            window.Show();
            Application.Current.MainWindow.Close();*/
        }
    }
}
