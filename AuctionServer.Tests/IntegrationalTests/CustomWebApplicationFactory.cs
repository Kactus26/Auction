using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;


namespace Auction.Tests.IntegrationalTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program> 
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
        }
    }
}
