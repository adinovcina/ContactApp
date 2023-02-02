using System.Text.Json.Serialization;

namespace ContactApp.Models.ApiModels
{
    public class ContactApiResponse
    {
        [JsonPropertyName("error_code")]
        public int? ErrorCode { get; set; } = 0;

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<Contact>? Data { get; set; }
    }
}
