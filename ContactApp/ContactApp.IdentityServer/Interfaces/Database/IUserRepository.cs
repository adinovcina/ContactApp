namespace ContactApp.IdentityServer.Interfaces.Database
{
    public interface IUserRepository
    {
        Task CreateUserAsync(string email, string password); 
    }
}
