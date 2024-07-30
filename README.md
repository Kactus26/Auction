Small documentation about how some things work
First of all, Auction is a microservice multiplatform app that uses ASP.Net and WPF. Right now, the app has 4 services (AuctionClient - WPF client, AuctionGateway, AuctionIdentity - for Authorization and handling all things related to it, AuctionServer - the main server service that has most of the data), a solution for testing, and a library with common DTOs. The Gateway is used for authentication checking (when a request comes to the gateway, it takes the token from the request header and checks if it passes validation, meaning it's not expired and has the correct SigningKey), routing, and load balancing.

AuctionClient<br>
AuctionClient is a WPF application. It consists of a registration window and a "MainWindow". MainWindow is basically a hollow template with a TabControl, in which are placed TabItems that are made from UserControls (WPF elements that you can paste into other pages). It also has MaterialDesign connected, for better visual decoration. It's made using the MVVM pattern, and for that reason, every Window/UserControl has its own ViewModel class with actions and properties. I use CommunityToolkit.MVVM that simplifies its realization (you can just use attributes for View pages to "see" your methods or properties). Methods that need server data are sent to the gateway and from it to the required microservice and controller in it. Interactions between the microservices work with the help of HttpClient.

In the images folder, we have 2 types of images:

Images that are constant (for example, a guest profile image).
Images that the server sends (profile images that the user uploads and saves).
Every ViewModel contains a generic post/get method that sends data to the gateway. Other methods provide the name of the gateway method as a parameter and the data it needs to transfer. This post/get method also checks the success of the response.

How Registration Works<br>
After the user enters all required data for registration and it has been validated successfully, the request through the gateway goes to the IdentityService. There, it checks for user login and email uniqueness, hashes the password using the BCrypt library (PasswordHasherService), adds the user to the AuctionIdentity SQLServer database (which keeps only the login, hashed password, and email), and finally generates a token using SigningCredentials that makes a SymmetricSecurityKey based on a string in appsettings.json. That token is sent back to the client, where it is saved in a SQLite local database. The token contains a userId and expires in 12 hours.

Every time the app launches, it checks if the token exists and is not expired, and if everything is correct, it launches the MainWindow without the need to log in manually. When the user exits their account, the token is deleted from the local database. If the user chooses to enter as a guest, they won't be able to use some functions. On the program side, they just won't have a token, so gateway methods that are marked as Authorized will send the request back.

All data is sent in JSON format, so deserialization is done using Newtonsoft.Json. When a user needs to send an image, it is converted into a byte array, and on the server side, a new image is created from this array using FileStream. When both data and an image need to be sent, it is converted into a class that has data and a byte array.

Backend Architecture<br>
Every ASP solution is made with MVC, DI, and the Repository pattern and has AutoMapper in it.

Testing<br>
In Auction.Test are located Unit and Integration tests. Integration tests connect to the real database, and after testing is done, roll back all changes. It has my own ChangeProperties method that, with the help of reflection, returns the object to its initial state. In this project, Integration tests also have the FluentAssertions library connected for better result checking. WebApplicationFactory is used there for the work of HttpClient. IntegrationTestBase has all necessary info for JWT creation work. For the work of Unit Tests, XUnit and Moq were connected, but I personally think that integration tests are more useful. For that reason, I have only a few unit tests, which I made just for learning purposes.
