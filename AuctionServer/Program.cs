using AuctionServer.Data;
using Microsoft.EntityFrameworkCore;
using SignalRTest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddTransient<Seed>();


builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    await new Seed(scope.ServiceProvider.GetRequiredService<DataContext>()).SeedDataContext();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapHub<ChatHub>("/chat");

app.Run();
