using AutoMapper;
using ContactApp.Api.Controllers.Contact.Models;
using ContactApp.Services.Interfaces;

namespace ContactApp.Api.Controllers.Contact.ControllerService
{
    public class ContactControllerService : IContactControllerService
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactControllerService(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactApp.Models.Contact>> GetAllContactsAsync(ContactSearchObject searchObject, Guid userId)
        {
            var search = _mapper.Map<ContactApp.Models.ContactSearchDto>(searchObject);
            search.UserId = userId;
            return await _contactService.GetAllContactsAsync(search);
        }

        public async Task<IEnumerable<ContactApp.Models.Contact>> GetFavouritesAsync(Guid userId)
        {
            return await _contactService.GetFavouritesAsync(userId);
        }

        public async Task<ContactApp.Models.Contact> GetContactDetailsAsync(Guid contactId, Guid userId)
        {
            return await _contactService.GetContactDetailsAsync(contactId, userId);
        }

        public async Task CreateContactAsync(ContactDto model, Guid userId)
        {
            var contact = _mapper.Map<ContactApp.Models.Contact>(model);
            contact.UserId = userId;
            await _contactService.CreateContactAsync(contact);
        }

        public async Task<ContactApp.Models.Contact> EditContactAsync(ContactDto model, Guid userId)
        {
            if (!model.Id.HasValue)
            {
                throw new ArgumentException("Id must be provided");
            }
            var contact = _mapper.Map<ContactApp.Models.Contact>(model);
            contact.UserId = userId;
            return await _contactService.EditContactAsync(contact);
        }

        public async Task DeleteContactAsync(Guid contactId, Guid userId)
        {
            await _contactService.DeleteContactAsync(contactId, userId);
        }
    }
}
