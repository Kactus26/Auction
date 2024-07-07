using AuctionIdentity.Interfaces;
using AuctionIdentity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionServer.Tests.IntegrationalTests
{
    public class IntegrationTestBase
    {
        protected readonly IJWTProvider _jwtProvider;
        public IntegrationTestBase()
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("C:\\Users\\Professional\\source\\repos\\Auction\\AuctionServer.Tests\\appsettings.json") // Убедитесь, что у вас есть этот файл с нужными настройками
           .Build();

            var services = new ServiceCollection();
            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<IJWTProvider, JWTProvider>(); 

            var serviceProvider = services.BuildServiceProvider();
            _jwtProvider = serviceProvider.GetRequiredService<IJWTProvider>() as JWTProvider;
        }
    }
}
