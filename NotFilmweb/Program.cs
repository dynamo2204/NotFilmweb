using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotFilmweb.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await NotFilmweb.Data.DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine("B³¹d podczas tworzenia ról: " + ex.Message);
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NotFilmweb.Data.ApplicationDbContext>();

        // TA LINIA JEST NOWA - Automatycznie wykonuje "Update-Database" przy starcie
        context.Database.Migrate();

        // Uruchomienie Seedera (tworzenie ról i admina)
        await NotFilmweb.Data.DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine("B³¹d podczas inicjalizacji bazy: " + ex.Message);
    }
}

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
