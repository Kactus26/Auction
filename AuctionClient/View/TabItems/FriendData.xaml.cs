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
    /// Логика взаимодействия для FriendData.xaml
    /// </summary>
    public partial class FriendData : UserControl
    {
        public FriendData()
        {
            InitializeComponent();
        }

        private void Close_Tab(object sender, RoutedEventArgs e)
        {
            var mainControl = FindParent<MainWindow>(this);

            if (mainControl != null)
            {
                // Ищем вкладку с заданным именем
                mainControl.RemoveNewTab(Name.Text);
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
