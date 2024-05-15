using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AuctionClient.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        [ObservableProperty]
        private string text = "sdsafafasf";

        [RelayCommand]
        public void Test()
        {
            Text = "dgfgdsgsd";
        }
    }
}
