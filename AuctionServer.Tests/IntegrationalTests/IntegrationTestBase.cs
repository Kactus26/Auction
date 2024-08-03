using Auction.Tests.IntegrationalTests.AuctionIdentityTests.Controllers;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Services;
using AuctionServer.Data;
using AuctionServer.Tests.IntegrationalTests.AuctionServerTests.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuctionServer.Tests.IntegrationalTests
{
    public class IntegrationTestBase
    {
        protected readonly IJWTProvider _jwtProvider;
        protected readonly DataContext _dataContext;
        protected readonly AuctionIdentity.Data.DataContext _dataContextIdentity;
        public IntegrationTestBase()
        {
            string jsonPath = Path.GetFullPath(@"..\..\..\appsettings.json");

            var configuration = new ConfigurationBuilder()
           .AddJsonFile(jsonPath)
           .Build();

            var services = new ServiceCollection();


            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<IJWTProvider, JWTProvider>();
            ServiceProvider serviceProvider;

            if (this is DataControllerTests)
            {
                services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AuctionDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));
                serviceProvider = services.BuildServiceProvider();
                _dataContext = serviceProvider.GetRequiredService<DataContext>();
            }
            else if (this is UserControllerTests)
            {
                services.AddDbContext<AuctionIdentity.Data.DataContext>(options =>
                    options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AuctionIdentityDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));
                serviceProvider = services.BuildServiceProvider();
                _dataContextIdentity = serviceProvider.GetRequiredService<AuctionIdentity.Data.DataContext>();
            }
            else
                serviceProvider = services.BuildServiceProvider();

            _jwtProvider = serviceProvider.GetRequiredService<IJWTProvider>() as JWTProvider;
        }
    }
}
