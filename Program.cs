using MyApiProject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyApiProject.Services;
using Microsoft.EntityFrameworkCore;
using MyApiProject.Data;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddScoped<IActionsinterface, Actions>();
builder.Services.AddScoped<ILogininterface, LoginService>();
builder.Services.AddScoped<IUserService, UserService>();

var jwtSettings = configuration.GetSection("JwtSettings");
Console.WriteLine(jwtSettings);
var secretKey = jwtSettings["key"] ?? throw new InvalidOperationException("JWT key is missing");
var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT issuer is missing");
var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT audience is missing");


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
}).AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

builder.Services.AddAuthorization();


builder.Services.AddDistributedMemoryCache();
// OR, for Production (Redis Cache)

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "RefreshTokens_";
});




builder.Services.AddDbContext<ApplicationDbContext>(options => // <--- FIXED
{
    // Use the connection string defined in appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
