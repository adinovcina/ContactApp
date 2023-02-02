using AutoMapper;
using FluentValidation;
using System.Text.Json.Serialization;

namespace ContactApp.Api.Controllers.Contact.Models
{
    public class ContactDto
    {
        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("phone")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("is_favourite")]
        public bool IsFavourite { get; set; }

        [JsonPropertyName("user_id")]
        public Guid? UserId { get; set; }

        public static void CreateMap(Profile mapper)
        {
            mapper.CreateMap<ContactDto, ContactApp.Models.Contact>();
        }
    }

    public class ContactDtoValidator : AbstractValidator<ContactDto>
    {
        public ContactDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[\d\s\-\(\)]{7,15}")
                .WithMessage("Invalid phone number")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));
        }
    }
}
