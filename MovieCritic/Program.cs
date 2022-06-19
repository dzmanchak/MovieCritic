using Authorization.Data;
using Authorization.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<Seeder>();
builder.Services.AddTransient<GenreManager>();
builder.Services.AddTransient<MovieManager>();
builder.Services.AddTransient<ReviewManager>();

var app = builder.Build();

//Seeder
var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

using (var scope = scopedFactory.CreateScope())
{
    var service = scope.ServiceProvider.GetService<Seeder>();
    service.Seed();
}


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
    pattern: "{controller=Movie}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
