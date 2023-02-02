using ContactApp.Data.EF.Identity;
using ContactApp.IdentityServer.Interfaces.Database;
using Microsoft.AspNetCore.Identity;

namespace ContactApp.IdentityServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateUserAsync(string email, string password)
        {
            await VerifyIfUserExistsAsync(email);
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception("Error occured");
            }
        }

        private async Task VerifyIfUserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                throw new InvalidOperationException("Invalid request");
            }
        }
    }
}
