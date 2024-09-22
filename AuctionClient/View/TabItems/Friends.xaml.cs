using AuctionClient.ViewModel.TabItems;
using CommonDTO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static AuctionClient.ViewModel.TabItems.FriendsViewModel;

namespace AuctionClient.View.TabItems
{
    /// <summary>
    /// Логика взаимодействия для Friends.xaml
    /// </summary>
    public partial class Friends : UserControl
    {
        public Friends()
        {
            InitializeComponent();
        }

        private void UsersListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView1.SelectedItem is UserDataWithImageDTO selectedUserList1)
            {
                AddNewTab(selectedUserList1);
            } 
        }

        private void UsersListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView2.SelectedItem is UserDataWithImageDTO selectedUserList2)
                AddNewTab(selectedUserList2);
        }

        private void AddNewTab(UserDataWithImageDTO selectedUser)
        {
            string userName = selectedUser.ProfileData.Name;

            var friendsDataVM = new FriendDataViewModel(selectedUser);

            FriendData newProfile = new FriendData() { DataContext = friendsDataVM };

            var mainControl = FindParent<MainWindow>(this);

            if (mainControl != null)
            {
                mainControl.AddNewTab(newProfile, userName);
            }
        }


        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            
            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
    }
}
