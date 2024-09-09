using CommonDTO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AuctionClient.ViewModel.TabItems
{
    public partial class FriendDataViewModel : ObservableObject
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
        public byte[] image;
        #endregion
        public FriendDataViewModel(UserDataWithImageDTO userData)
        {
            Name = userData.ProfileData.Name;
            SurName = userData.ProfileData.Surname;
            Email = userData.ProfileData.Email;
            Description = userData.ProfileData.Info;
            Image = userData.Image;
        }
    }
}
