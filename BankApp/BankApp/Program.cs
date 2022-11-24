using BankApp.Data;
using BankApp.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var DBconnectionString = builder.Configuration.GetConnectionString("LoansComparer");
builder.Services.AddDbContext<LoansComparerDBContext>(options =>
    options.UseSqlServer(DBconnectionString));
builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<INotRegisteredInquiryRepository, NotRegisteredInquiryRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ClientModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LoansComparerDBContext>();
builder.Services.AddControllersWithViews();

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
