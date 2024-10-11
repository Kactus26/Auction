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

namespace AuctionClient.View.TabItems
{
    /// <summary>
    /// Логика взаимодействия для AllLots.xaml
    /// </summary>
    public partial class AllLots : UserControl
    {
        public AllLots()
        {
            InitializeComponent();
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView1.SelectedItem is LotWithImageDTO selectedUserList1)
            {
                AddNewTab(selectedUserList1);
            }
        }

        private void ListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListView2.SelectedItem is LotWithImageDTO selectedUserList2)
                AddNewTab(selectedUserList2);
        }

        private async Task AddNewTab(LotWithImageDTO selectedLot)
        {
            string lotName;

            if (selectedLot.LotInfo.Name.Length > 10)
                lotName = selectedLot.LotInfo.Name.Substring(0, 9) + "...";
            else
                lotName = selectedLot.LotInfo.Name;

            var lotVM = new LotViewModel();

            Lot newLotPage = new() { DataContext = lotVM };

            await lotVM.InitializeAync(selectedLot);

            var mainControl = FindParent<MainWindow>(this);

            if (mainControl != null)
            {
                mainControl.AddNewTab(newLotPage, lotName);
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
