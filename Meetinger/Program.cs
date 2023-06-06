using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Meetinger.Areas.Identity.Data;
using Meetinger.Services;
using Meetinger.Models;
using React.AspNet;
using JavaScriptEngineSwitcher.V8;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Hangfire;
using Hangfire.SqlServer;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMeeting, MeetingService>();
builder.Services.AddScoped<List<Notification>>(_ => new List<Notification>());
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddReact();
builder.Services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName).AddV8();
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString);
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// app.UseReact(config =>
// {
//     config.AddScript("~/Scripts/HelloWorld.jsx");
// });
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseHangfireServer();
app.UseHangfireDashboard();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
