using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Profile_app_backend.Data;
using System.IdentityModel.Tokens.Jwt;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. Add Microsoft Identity Web API authentication scheme targeting B2C
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            //.WithOrigins(
            //    "http://localhost:5173",
            //    "https://yellow-ground-0dfad6c1e.2.azurestaticapps.net"
            //)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Ensure configuration for Entra / Azure AD is available. Prefer new "AzureAd" section
// (used by Microsoft.Identity.Web for Entra External ID). If only the older
// AzureAdB2C section exists, copy relevant values at runtime so the app keeps working
// while switching to Entra External ID.
if (string.IsNullOrEmpty(builder.Configuration["AzureAd:ClientId"]))
{
    var b2c = builder.Configuration.GetSection("AzureAdB2C");
    if (b2c.Exists())
    {
        var map = new Dictionary<string, string?>
        {
            ["AzureAd:Instance"] = b2c["Instance"],
            ["AzureAd:Domain"] = b2c["Domain"],
            ["AzureAd:ClientId"] = b2c["ClientId"]
        };
        // Configuration in WebApplicationBuilder is a ConfigurationManager and supports AddInMemoryCollection
        builder.Configuration.AddInMemoryCollection(map!);
    }
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration, "AzureAd");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
