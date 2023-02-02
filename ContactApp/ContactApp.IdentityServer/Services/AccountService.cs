using ContactApp.IdentityServer.Interfaces;
using ContactApp.IdentityServer.Interfaces.Database;

namespace ContactApp.IdentityServer.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;

        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUserAsync(string email, string password)
        {
            await _userRepository.CreateUserAsync(email, password);
        }
    }
}
