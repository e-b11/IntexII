using dotenv.net;
using IntexII.Data;
using IntexII.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
DotEnv.Load();

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
});


builder.Services.AddScoped<IIntexRepository, EFIntexRepository>();


builder.Services.AddRazorPages();

//Uncomment these as things are built
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout to 30 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

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

//CSP Header
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; img-src 'self' https://live.staticflickr.com;");
    await next();
});


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute("pagenumandtype", "Products/{category}/Page/{pageNum}", new { Controller = "Home", Action = "BrowseProducts" });
app.MapControllerRoute("page", "Products/Page/{pageNum}", new { Controller = "Home", Action = "BrowseProducts", pageNum = 1 });
app.MapControllerRoute("category", "Products/{category}", new { Controller = "Home", Action = "BrowseProducts", pageNum = 1 });
app.MapControllerRoute("pagination", "Products/Page/{pageNum}", new { Controller = "Home", Action = "BrowseProducts", pageNum = 1 });

app.MapDefaultControllerRoute();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRolesAsync(userManager, roleManager);
    // await SeedAdminUserAsync(userManager, roleManager);
}
async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var adminRole = new IdentityRole("Admin");
        await roleManager.CreateAsync(adminRole);
    }
    if (!await roleManager.RoleExistsAsync("User"))
    {
        var userRole = new IdentityRole("User");
        await roleManager.CreateAsync(userRole);
    }
}

app.Run();
