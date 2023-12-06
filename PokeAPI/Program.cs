using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;
using PokeAPI.Services.DbContexts;

var builder = WebApplication.CreateBuilder(args);

Env.Load("Env/.env.local");
//Env.Load("Env/.env.docker");

var redisHost = Environment.GetEnvironmentVariable("REDIS_HOST");
var redisPort = Environment.GetEnvironmentVariable("REDIS_PORT");
var emailerLogin = Environment.GetEnvironmentVariable("EMAILER_LOGIN");
var emailerPassword = Environment.GetEnvironmentVariable("EMAILER_PASSWORD");
var ftpHost = Environment.GetEnvironmentVariable("FTP_HOST");

var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var dbName = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

var yandexClientId = Environment.GetEnvironmentVariable("YANDEX_CLIENT_ID");
var yandexRedirectUri = Environment.GetEnvironmentVariable("YANDEX_REDIRECT_URI");
var yandexTokenPageOrigin = Environment.GetEnvironmentVariable("YANDEX_TOKEN_PAGE_ORIGIN");


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });
builder.Services.AddSingleton(provider => new YandexApi()
{
    ClientId = yandexClientId,
    RedirectUri = yandexRedirectUri,
    TokenPageOrigin = yandexTokenPageOrigin
});
builder.Services.AddDbContext<PokeDbContext>(options => options.UseNpgsql($"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword}"));
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(provider => new Emailer(emailerLogin, emailerPassword));
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
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