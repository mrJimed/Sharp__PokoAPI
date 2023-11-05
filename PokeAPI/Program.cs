using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;
using PokeAPI.Services.DbContexts;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
string emailerLogin = builder.Configuration.GetSection("EmailerSetting").GetSection("login").Value;
string emailerPassword = builder.Configuration.GetSection("EmailerSetting").GetSection("password").Value;
string ftpHost = builder.Configuration.GetSection("FtpSettings").GetSection("host").Value;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
    });
builder.Services.AddDbContext<PokeDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(provider => new Emailer(emailerLogin, emailerPassword));
builder.Services.AddTransient<IFtpService>(provider => new FtpService(ftpHost));
builder.Services.AddTransient<IFightStatService, FightStatService>();
builder.Services.AddTransient<IUserService, UsersService>();
builder.Services.AddTransient<IPokeApi, PokeApi>();
builder.Services.AddSingleton<IFileProvider>(
        new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

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