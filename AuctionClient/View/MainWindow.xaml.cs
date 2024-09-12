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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
