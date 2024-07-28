Small documentation about how some things work.

Auction Client
AuctionClient is a WPF application. It consists of a registration window and "MainWindow". MainWindow is basically a hollow template with a TabControl, in which are placed TabItems that are made from UserControls (WPF elements that you can paste into other pages).
It's made using the MVVM pattern, and for that reason, every Window/UserControl has its own ViewModel class with actions and properties.
We use SQLite for saving the user JWT token, which has a userId in it and expires in 12 hours. Other data is kept on SQL Server in other services.
In the images folder we have 2 types of images:
Images that are constant (for example, a guest profile image)
Images that the server sends (profile images that the user inserts and saves)
