using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CasinoRoyale.Data;
using CasinoRoyale.Models; // Add this for your User class

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity services with your custom User class
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Seed admin user & roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedAdminUser(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding admin user: {ex.Message}");
    }
}

// Middleware
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

async Task SeedAdminUser(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminEmail = "admin@slotrush.com";
    string adminPassword = "Admin123!"; // Change for production
    string adminRole = "Admin";
    string playerRole = "Player";

    // Create Admin role if it doesn't exist
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // Create Player role if it doesn't exist
    if (!await roleManager.RoleExistsAsync(playerRole))
    {
        await roleManager.CreateAsync(new IdentityRole(playerRole));
    }

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
            Console.WriteLine("Admin user created");
        }
        else
        {
            Console.WriteLine("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        Console.WriteLine("Admin user already exists");
    }
}
