using System.Windows;
using System.Windows.Controls;

namespace AuctionClient.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddNewTab(UserControl userControl, string header)
        {
            TabItem newTabItem = new TabItem()
            {
                Header = header,
                Content = userControl                
            };

            MainTabControl.Items.Add(newTabItem);
            MainTabControl.SelectedItem = newTabItem;
        }

        public void RemoveNewTab(string header)
        {
            var tabToRemove = MainTabControl.Items
            .OfType<TabItem>()
            .FirstOrDefault(tab => tab.Header.ToString() == header);

            MainTabControl.Items.Remove(tabToRemove);
        }
    }
}
