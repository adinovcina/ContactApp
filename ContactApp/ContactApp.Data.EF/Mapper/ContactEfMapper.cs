using AutoMapper;

namespace ContactApp.Data.EF.Mapper
{
    public class ContactEfMapper : Profile
    {
        public ContactEfMapper()
        {
            CreateMap<Models.Contact, Entities.Contact>().ReverseMap();
        }
    }
}
