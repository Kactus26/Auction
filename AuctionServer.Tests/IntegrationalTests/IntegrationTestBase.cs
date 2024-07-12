using AuctionIdentity.Interfaces;
using AuctionIdentity.Services;
using AuctionServer.Data;
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

            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AuctionDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));

            var serviceProvider = services.BuildServiceProvider();
            _jwtProvider = serviceProvider.GetRequiredService<IJWTProvider>() as JWTProvider;
            _dataContext = serviceProvider.GetRequiredService<DataContext>();
        }
    }
}
