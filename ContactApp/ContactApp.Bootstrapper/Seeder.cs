using ContactApp.Data.EF.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ContactApp.Bootstrapper
{
    public static class Seeder
    {
        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                if (!dbContext.Database.CanConnect())
                {
                    Environment.Exit(-1);
                }
                dbContext.Database.Migrate();
            }
            return host;
        }
    }
}
