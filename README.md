<b><h1>Small documentation about how some things work</h1></b>
First of all, Auction is a microservice multiplatform app that uses ASP.Net and WPF. Right now, the app has 4 services (AuctionClient - WPF client, AuctionGateway, AuctionIdentity - for Authorization and handling all things related to it, AuctionServer - the main server service that has most of the data), a solution for testing, and a library with common DTOs.<br><br>
Every ASP solution is made with MVC, DI, and the Repository pattern and has AutoMapper in it. The Gateway is used in every request to any other server, for authentication checking (when a request comes to the gateway, it takes the token from the request header and checks if it passes validation, meaning it's not expired and has the correct SigningKey), routing, and load balancing. <br><br>
All data is sent in JSON format, so deserialization is done using Newtonsoft.Json. When a user needs to send an image, it is converted into a byte array, and on the server side, a new image is created from this array using FileStream. When both data and an image need to be sent, it is converted into a class that has both data and a byte array.<br><br>
Some ViewModel contains a generic post/get method that sends data to the gateway. Other methods provide the name of the gateway method as a parameter and the data it needs to transfer. This post/get method also checks the success of the response.

<b><h1>AuctionClient</h1></b>
It consists of a registration window and a "MainWindow". MainWindow is basically a hollow template with a TabControl, in which are placed TabItems that are made from UserControls (WPF elements that you can paste into other pages). It also has MaterialDesign connected, for better visual decoration. It's made using the MVVM pattern, and for that reason, every Window/UserControl has its own ViewModel class with actions and properties. I use CommunityToolkit.MVVM that simplifies its realization (you can just use attributes for View pages to "see" your methods or properties). Methods that need server data are sent to the gateway and from it to the required microservice and controller in it. Interactions between the microservices work with the help of HttpClient.

<b><h1>Email confirmation</h1></b>
You can confirm email only after your account is created on your profile page.

<h3>Client side</h3>
Email and it's IsConfirmed status keeps on the AuctionServer db, so after your account page opens, neitherless your email is not confirmed, it will be written in email field, with the label "Not confirmed" on the top of it. After you click "Confirm Email" button, opens a new window woth text field in which you need to enter the code from the email that was send to the mail addess which was placed in the field. If user closes this window, it's automatically considers as NotConfirmed and writes it in MessageBox. Required password provided to this window on it's creation(whatch server side below), if user writes it correctly, window will close, new request will be send and email status will be marked as Confirmed.<br>

<h3>Server side</h3>
After user taps "Confirm Email" button, request with email address sends to the AuctionIdnetity, where the MailService with the help of the MailKit library sends mail to required account. Mail contains randomly generated passwords from 6 numbers. The copy of that password sends back to the client with successfull mail sending info.

<b><h1>Registration</h1></b>
After the user enters all required data for registration and it has been validated successfully, the request goes to the IdentityService. There, it checks for user login and email uniqueness, hashes the password using the BCrypt library (PasswordHasherService), adds the user to the AuctionIdentity SQLServer database (which keeps only the login, hashed password, and email), and finally generates a token using SigningCredentials that makes a SymmetricSecurityKey based on a string in appsettings.json. That token is sent back to the client, where it is saved in a SQLite local database. The token contains a userId and expires in 12 hours.<br>

Every time the app launches, it checks if the token exists and is not expired, and if everything is correct, it launches the MainWindow without the need to log in manually. When the user exits their account, the token is deleted from the local database. If the user chooses to enter as a guest, they won't be able to use some functions. On the program side, they just won't have a token, so gateway methods that are marked as Authorized will send the request back.

<b><h1>Testing</h1></b>
In Auction.Test are located Unit and Integration tests. Integration tests connect to the real database, and after testing is done, roll back all changes. It has my own ChangeProperties method that, with the help of reflection, returns the object to its initial state. In this project, Integration tests also have the FluentAssertions library connected for better result checking. WebApplicationFactory is used there for the work of HttpClient. IntegrationTestBase has all necessary info for JWT creation work. For the work of Unit Tests, XUnit and Moq were connected, but I personally think that integration tests are more useful. For that reason, I have only a few unit tests, which I made just for learning purposes.
