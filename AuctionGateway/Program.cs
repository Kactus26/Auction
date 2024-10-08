using CommonDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsProduction())
{
    builder.Configuration
        .AddJsonFile("appsettings.gw.Production.json", optional: true, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.gw.Development.json", optional: true, reloadOnChange: true);
}

builder.Services.AddHttpClient("IdentityServer", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("IdentityServerUrl")! + "/api/");
});
builder.Services.AddHttpClient("AuctionServer", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("AuctionServerUrl")! + "/api/");
});


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (builder.Configuration.GetRequiredSection("JWTOptions").GetValue<string>("SecretKey")!))
                    };
                });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
