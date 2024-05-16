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
        private string text = "sdsafafasf";

        [RelayCommand]
        public void Test()
        {
            MainWindow window = new MainWindow();
            window.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
