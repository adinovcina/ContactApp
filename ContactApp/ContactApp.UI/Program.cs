using ContactApp.UI;
using ContactApp.UI.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                        // Set Authority to setting in appsettings.json. This is the URL of the IdentityServer4
                        options.Authority = builder.Configuration.GetValue<string>("OIDC:Authority");

                        // Set ClientId to setting in appsettings.json.
                        options.ClientId = builder.Configuration.GetValue<string>("OIDC:ClientId");

                        // Set ClientSecret to setting in appsettings.json.
                        options.ClientSecret = builder.Configuration.GetValue<string>("OIDC:ClientSecret");

                        // When set to code, the middleware will use PKCE protection
                        options.ResponseType = "code";

                        // Add request scopes
                        options.Scope.Add("openid");
                        options.Scope.Add("ContactApp.Api");
                        options.Scope.Add("offline_access");

                        // Removed scope that was added by default

                        options.Scope.Remove("profile");

                        // Save access and refresh tokens to authentication cookie, the default is false
                        options.SaveTokens = true;

                        // It's recommended to always get claims from the 
                        // UserInfoEndpoint during the flow. 
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };
                    });

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();
builder.Services.AddHttpClient<IContactService, ContactService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseUrl"));
    });

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
