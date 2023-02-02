using AutoMapper;
using System.Text.Json.Serialization;

namespace ContactApp.Api.Controllers.Contact.Models
{
    public class ContactSearchObject
    {
        [JsonPropertyName("filter")]
        public string? SearchPhrase { get; set; }

        [JsonPropertyName("skip")]
        public int? Skip { get; set; }

        [JsonPropertyName("take")]
        public int? Take { get; set; }

        public static void CreateMap(Profile mapper)
        {
            mapper.CreateMap<ContactSearchObject, ContactApp.Models.ContactSearchDto>().ReverseMap();
        }
    }
}
