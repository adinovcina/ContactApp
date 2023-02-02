using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactApp.IdentityServer.Requests
{
    public class RegisterRequest
    {
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
