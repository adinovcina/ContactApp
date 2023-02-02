using ContactApp.Data.EF.EfCore;
using ContactApp.Data.EF.Identity;
using ContactApp.IdentityServer;
using ContactApp.IdentityServer.Config;
using ContactApp.IdentityServer.Interfaces;
using ContactApp.IdentityServer.Interfaces.Database;
using ContactApp.IdentityServer.Repositories;
using ContactApp.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database connection
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

// Load dependencies
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

// Add identity server
var settings = builder.Configuration.GetSection("Identity").Get<Settings>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryIdentityResources(builder.Configuration.GetSection("IdentityResources"))
    .AddInMemoryApiResources(builder.Configuration.GetSection("ApiResources"))
    .AddInMemoryApiScopes(builder.Configuration.GetSection("ApiScopes"))
    .AddInMemoryClients(Config.GetClients(settings))
    .AddDeveloperSigningCredential();

// Add authorization
builder.Services.AddAuthorization();

// Add authentication
builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.SeedData();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

app.Run();
