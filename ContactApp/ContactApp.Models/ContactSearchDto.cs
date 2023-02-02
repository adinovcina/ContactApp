using System.Text.Json.Serialization;

namespace ContactApp.Models
{
    public class ContactSearchDto
    {
        [JsonPropertyName("filter")]
        public string? SearchPhrase { get; set; }

        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("take")]
        public int? Take { get; set; }
    }
}
