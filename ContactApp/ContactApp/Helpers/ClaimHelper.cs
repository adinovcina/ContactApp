using System.Security.Claims;

namespace ContactApp.Api.Helpers
{
    public class ClaimHelper
    {
        public static Guid GetUserIdFromClaim(ClaimsPrincipal user, string type)
        {
            return new Guid(user.Claims.Single(x => x.Type == type).Value);
        }
    }
}
