using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactApp.Models
{
    public class Contact
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("name")]
        [Required]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        [RegularExpression(@"^\+?[\d\s\-\(\)]{7,15}", ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("is_favourite")]
        public bool IsFavourite { get; set; }

        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
    }
}