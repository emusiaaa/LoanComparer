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
using BankApp.Client;
using IdentityModel.Client;
using System.Net.Http;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using BankApp;

var builder = WebApplication.CreateBuilder(args);

var DBconnectionString = builder.Configuration.GetConnectionString("LoansComparer");
builder.Services.AddScoped<MiNIApiCaller>();
builder.Services.AddScoped<BestAPIApiCaller>();
builder.Services.AddScoped<StrangerApiCaller>();

builder.Services.AddAccessTokenManagement(options =>
{
    options.Client.Clients.Add("identityserver", new ClientCredentialsTokenRequest
    {
        Address = "https://indentitymanager.snet.com.pl/connect/token",
        ClientId = "team4c",
        ClientSecret = "7D84D860-87AC-46AE-B955-68DC7D8C48E3"
    });
}).ConfigureBackchannelHttpClient();

builder.Services.AddHttpClient<MiNIApiCaller>(client =>
{
    client.BaseAddress = new Uri("https://mini.loanbank.api.snet.com.pl/swagger/index.html");
})
    .AddClientAccessTokenHandler("identityserver");

builder.Services.AddHttpClient<BestAPIApiCaller>(client =>
{
    client.BaseAddress = new Uri("https://best-bank-webapi.azurewebsites.net");
});

builder.Services.AddHttpClient<StrangerApiCaller>(client =>
{
    client.BaseAddress = new Uri("https://fictionbank20230111105722.azurewebsites.net");
});

builder.Services.AddDbContext<LoansComparerDBContext>(options =>
    options.UseSqlServer(DBconnectionString));
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<INotRegisteredInquiryRepository, NotRegisteredInquiryRepository>();
builder.Services.AddScoped<ILoggedInquiryRepository, LoggedInquiryRepository>();
builder.Services.AddScoped<IOffersSummaryRepository, OffersSummaryRepository>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ClientModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LoansComparerDBContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddCloudscribePagination();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});


builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IOfferServer, OfferServer>()
    .AddSingleton<IServiceProvider,ServiceProvider>();
builder.Services.AddScoped<IInquiryServer,InquiryServer>();

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
