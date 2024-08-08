using CommunityToolkit.Mvvm.ComponentModel;

namespace AuctionClient.ViewModel
{
    internal class ConfirmEmailViewModel : ObservableObject
    {
        private readonly string password;
        public ConfirmEmailViewModel(string password)
        {
            this.password = password;
        }
    }
}
