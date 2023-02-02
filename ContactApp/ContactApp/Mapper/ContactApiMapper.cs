using AutoMapper;
using ContactApp.Api.Controllers.Contact.Models;

namespace ContactApp.Api.Mapper
{
    public class ContactApiMapper : Profile
    {
        public ContactApiMapper()
        {
            ContactDto.CreateMap(this);
            ContactSearchObject.CreateMap(this);
        }
    }
}
