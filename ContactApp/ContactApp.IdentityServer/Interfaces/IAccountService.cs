namespace ContactApp.IdentityServer.Interfaces
{
    public interface IAccountService
    {
        Task CreateUserAsync(string email, string password);
    }
}
