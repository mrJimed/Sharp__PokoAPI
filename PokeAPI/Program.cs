using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PokeAPI.Services;
using PokeAPI.Services.DbContexts;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
string emailerLogin = builder.Configuration.GetSection("EmailerSetting").GetSection("login").Value;
string emailerPassword = builder.Configuration.GetSection("EmailerSetting").GetSection("password").Value;

builder.Services.AddDbContext<PokeDbContext>(options => options.UseNpgsql(connection));
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(provider => new Emailer(emailerLogin, emailerPassword));
builder.Services.AddTransient<IFightStatService, FightStatService>();
builder.Services.AddTransient<IPokeApi, PokeApi>();
builder.Services.AddSingleton<IFileProvider>(
        new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Index}");
app.Run();