using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using Serilog.Events;
using SimpleFreeBoard.Contexts;
using SimpleFreeBoard.Repositories;
using SimpleFreeBoard.Services;
using SimpleFreeBoard.Services.Security;

namespace SimpleFreeBoard;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {Message}{NewLine}{Exception}]") // 콘솔에 로그 기록
            .WriteTo.File("logs/board-.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<DapperContext>();
        builder.Services.AddSingleton<PasswordHasher>();
        builder.Services.AddScoped<AccountRepository>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        
        builder.Host.UseSerilog();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            var scope = app.Services.CreateScope();
            app.UseDeveloperExceptionPage();
            var context = scope.ServiceProvider.GetRequiredService<DapperContext>();
            DbSeeder.Seed(context);
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}