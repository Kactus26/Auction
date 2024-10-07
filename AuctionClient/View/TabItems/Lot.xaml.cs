using AuctionClient.ViewModel.TabItems;
using CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AuctionClient.View.TabItems
{
    /// <summary>
    /// Логика взаимодействия для Lot.xaml
    /// </summary>
    public partial class Lot : UserControl
    {
        public Lot()
        {
            InitializeComponent();
        }

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Проверяем, существует ли родительский ScrollViewer
            if (MainScrollViewer != null)
            {
                // Передаём событие прокрутки родительскому ScrollViewer
                MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset - e.Delta);
                e.Handled = true; // Предотвращаем дальнейшую обработку события ListView
            }
        }

        private void Close_Tab(object sender, RoutedEventArgs e)
        {
            var mainControl = FindParent<MainWindow>(this);

            if (mainControl != null)
            {
                string tabName;
                if (Name.Text.Length > 10)
                    tabName = Name.Text.Substring(0, 9) + "...";
                else
                    tabName = Name.Text;

                mainControl.RemoveNewTab(tabName);
            }
        }

        private void Owner_Selected(object sender, RoutedEventArgs e)
        {
            if (Owner.SelectedItem is UserDataWithImageDTO selectedUserList2)
                AddNewTab(selectedUserList2);
        }

        private void AddNewTab(UserDataWithImageDTO selectedUser)
        {
            string userName;

            if (selectedUser.ProfileData.Name.Length > 10)
                userName = selectedUser.ProfileData.Name.Substring(0,9) + "..."; 
            else
                userName = selectedUser.ProfileData.Name;

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
