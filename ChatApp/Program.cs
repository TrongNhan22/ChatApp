using ChatApp.Data;
using ChatApp.Hubs;
using ChatApp.Repository;
using ChatApp.Models;
using MongoDB.Bson;
using AspNetCore.Identity.Mongo.Model;
//using AspNetCore.Identity.Mongo;
using MongoDbGenericRepository.Attributes;
using ChatApp.Interface;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));

// This is required to ensure server can identify user after login
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<ISearchFriendRepository, SearchFriendRepository>();
builder.Services.AddScoped<ILogin, LoginRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUpdateUserRepository, UpdateUserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapBlazorHub();
app.MapHub<ChatHub>("/ChatHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
