using BestStoreMVC.Models;                            //Contains data classes like Product, Order, ApplicationUser.
using BestStoreMVC.Services;                          //Contains DbContext and helper services.
using Microsoft.AspNetCore.Identity;                  //Handles Login, Register, Roles.
using Microsoft.EntityFrameworkCore;                  //Used to connect and work with database.
using Microsoft.EntityFrameworkCore.Infrastructure;
using sib_api_v3_sdk.Client;                          // Used for sending emails via Brevo API.

var builder = WebApplication.CreateBuilder(args);   // This creates a builder object that configures the application  // It prepares services and settings before the app runs.

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});


//This registers the database context.
//It reads connection string from appsettings.json and connects to SQL Server.
//Login/Register system ready karto.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
	options =>
	{
		options.Password.RequiredLength = 6;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = false;
	})
    //ser data database madhe store karayla sangto.
    .AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

//Email sending sathi Brevo API key set karto.
//API key appsettings.json madhun gheto.	
Configuration.Default.ApiKey.Add("api-key", builder.Configuration["BrevoSettings:ApiKey"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
// Production madhe error page show karto
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// create the roles and the first admin user if not available yet
using (var scope = app.Services.CreateScope())
{
	var userManager = scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>))
		as UserManager<ApplicationUser>;
	var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
		as RoleManager<IdentityRole>;

	await DatabaseInitializer.SeedDataAsync(userManager, roleManager);
}
// Check karto roles exist kartat ka
// Nasel tar create karto (Admin, Customer etc.)
// First Admin user pan create karto


app.Run();
