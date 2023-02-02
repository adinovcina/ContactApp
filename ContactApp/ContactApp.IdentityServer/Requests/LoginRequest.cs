using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactApp.IdentityServer.Requests
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        [Required]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }

        [JsonPropertyName("return_url")]
        public string ReturnUrl { get; set; }
    }
}
