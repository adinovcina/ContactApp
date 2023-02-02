using ContactApp.Data.EF.Repositories.Contact;
using ContactApp.Services.Interfaces;
using ContactApp.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ContactApp.Bootstrapper
{
    public class Bootstrapper
    {
        private static readonly string scope = "ContactApp.Api";

        public static void LoadDependencies(IServiceCollection services)
        {
            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IContactRepository, ContactRepository>();
        }

        public static void AddPolicies(AuthorizationOptions options, string identityUrl)
        {
            options.AddPolicy("UserPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", scope);
                policy.RequireClaim("iss", identityUrl);
            });
        }
    }
}