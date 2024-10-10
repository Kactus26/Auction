using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AuctionClient.View.TabItems
{
    /// <summary>
    /// Логика взаимодействия для LotConstructor.xaml
    /// </summary>
    public partial class LotConstructor : UserControl
    {
        public LotConstructor()
        {
            InitializeComponent();
        }

        private void Close_Tab(object sender, RoutedEventArgs e)
        {
            var mainControl = FindParent<MainWindow>(this);

            if (mainControl != null)
                mainControl.RemoveNewTab("Create");

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
