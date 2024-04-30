using Microsoft.AspNet.SignalR.Client;
using System.Windows;


namespace Auction
{
    public partial class MainWindow : Window
    {
        HubConnection connection;
        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
              .WithUrl("https://localhost:7098/chat")
              .Build();
        }
    }
}