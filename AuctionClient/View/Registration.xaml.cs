using AuctionClient.Data;
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

namespace AuctionClient.View
{
    public partial class Registration : Window
    {

        ApplicationContext db = new ApplicationContext();
        public Registration()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
            }
        }

        private void PasswordChangedReg(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            { 
                ((dynamic)DataContext).PasswordReg = ((PasswordBox)sender).Password; 
            }
        }

        private void ConfPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((dynamic)DataContext).ConfPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
