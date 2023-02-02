using ContactApp.Models;

namespace ContactApp.UI.Data
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetAllContacts(string? searchTerm);
        Task<IEnumerable<Contact>> GetFavourites();
        Task<Contact> GetContactDetails(Guid id);
        Task DeleteContact(Guid id);
        Task CreateContact(Contact model);
        Task UpdateContact(Contact model);
    }
}
