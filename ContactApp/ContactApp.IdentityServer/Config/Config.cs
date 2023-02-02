using IdentityServer4.Models;

namespace ContactApp.IdentityServer.Config
{
    public static class Config
    {
        public static IEnumerable<Client> GetClients(Settings settings)
        {
            return new[]
            {
                new Client
                {
                    ClientId = settings.PasswordClientId,
                    ClientName = settings.ClientName,
                    ClientSecrets = { new Secret($"{settings.Secret}".Sha256()) },

                    IdentityTokenLifetime = 300,
                    AccessTokenLifetime = 3600,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = settings.AllowedScopes,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    AllowedCorsOrigins = settings.AllowedCorsOrigins,

                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,

                    RedirectUris = settings.RedirectUris,
                    PostLogoutRedirectUris = settings.PostLogoutRedirectUris,
                    UpdateAccessTokenClaimsOnRefresh = true,
                }
            };
        }
    }
}
