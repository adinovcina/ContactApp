using ContactApp.Models;

namespace ContactApp.Services.Interfaces
{
    public interface IContactService
    {
        Task CreateContactAsync(Contact model);
        Task<Contact> EditContactAsync(Contact model);
        Task DeleteContactAsync(Guid contactId, Guid userId);
        Task<IEnumerable<Contact>> GetAllContactsAsync(ContactSearchDto model);
        Task<Contact> GetContactDetailsAsync(Guid contactId, Guid userId);
        Task<IEnumerable<Contact>> GetFavouritesAsync(Guid userId);
    }
}
