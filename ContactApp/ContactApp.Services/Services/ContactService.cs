using ContactApp.Data.EF.Repositories.Contact;
using ContactApp.Models;
using ContactApp.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ContactApp.Services.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMemoryCache _memoryCache;
        private const string keyFavourites = "favourites";

        public ContactService(IContactRepository contactRepository, IMemoryCache memoryCache)
        {
            _contactRepository = contactRepository;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync(ContactSearchDto model)
        {
            return await _contactRepository.GetAllContactsAsync(model);
        }

        public async Task<IEnumerable<Contact>> GetFavouritesAsync(Guid userId)
        {
            if (_memoryCache.TryGetValue($"{keyFavourites}{userId}", out IEnumerable<Contact> contacts))
            {
                return contacts;
            }

            var favourites = await _contactRepository.GetFavouritesAsync(userId);
            _memoryCache.Set($"{keyFavourites}{userId}", favourites);
            return favourites;
        }

        public async Task<Contact> GetContactDetailsAsync(Guid contactId, Guid userId)
        {
            return await _contactRepository.GetContactDetailsAsync(contactId, userId);
        }

        public async Task CreateContactAsync(Contact model)
        {
            await _contactRepository.CreateContactAsync(model);
        }

        public async Task<Contact> EditContactAsync(Contact model)
        {
            return await _contactRepository.EditContactAsync(model);
        }

        public async Task DeleteContactAsync(Guid contactId, Guid userId)
        {
            await _contactRepository.DeleteContactAsync(contactId, userId);
        }
    }
}
