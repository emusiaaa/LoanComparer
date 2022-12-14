using BankApp.Data;
using BankApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using BankApp.Services;
using RestSharp;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using System.Security.Policy;
using System;

var builder = WebApplication.CreateBuilder(args);

var DBconnectionString = builder.Configuration.GetConnectionString("LoansComparer");
builder.Services.AddHttpClient("API", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://mini.loanbank.api.snet.com.pl/swagger/index.html");
    //httpClient.DefaultRequestHeaders.Authorization = 
});

builder.Services.AddDbContext<LoansComparerDBContext>(options =>
    options.UseSqlServer(DBconnectionString));
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<INotRegisteredInquiryRepository, NotRegisteredInquiryRepository>();
builder.Services.AddTransient<ILoggedInquiryRepository, LoggedInquiryRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ClientModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LoansComparerDBContext>();
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
