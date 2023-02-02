namespace ContactApp.IdentityServer.Config
{
    public class Settings
    {
        public string? PasswordClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Secret { get; set; }
        public List<string>? AllowedScopes { get; set; }
        public List<string>? AllowedCorsOrigins { get; set; }
        public List<string>? RedirectUris { get; set; }
        public List<string>? PostLogoutRedirectUris { get; set; }
    }
}
