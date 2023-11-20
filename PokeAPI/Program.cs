using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;
using PokeAPI.Services.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Env.Load("Env/.env.local");
Env.Load("Env/.env.docker");

string redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
string redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
string emailerLogin = Environment.GetEnvironmentVariable("EMAILER_LOGIN");
string emailerPassword = Environment.GetEnvironmentVariable("EMAILER_PASSWORD");
string ftpHost = Environment.GetEnvironmentVariable("FTP_HOST");

string dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
string dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
string dbName = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
string dbUser = Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
string dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });
builder.Services.AddDbContext<PokeDbContext>(options => options.UseNpgsql($"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}"));
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(provider => new Emailer(emailerLogin, emailerPassword));
builder.Services.AddTransient<IFtpService>(provider => new FtpService(ftpHost));
builder.Services.AddTransient<IFightStatService, FightStatService>();
builder.Services.AddTransient<IUserService, UsersService>();
builder.Services.AddTransient<IPokeApi, PokeApi>();
builder.Services.AddSingleton<IFileProvider>(
        new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = $"{redisHost}:{redisPort}";
    options.InstanceName = "local";
});

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Index}");
app.Run();