using ContactApp.Data.EF.EfCore;

namespace ContactApp.Data.EF.Repositories.Contact
{
    public interface IContactRepository : IRepository<Entities.Contact>
    {
        Task CreateContactAsync(Models.Contact contact);
        Task<Models.Contact> EditContactAsync(Models.Contact model);
        Task DeleteContactAsync(Guid contactId, Guid userId);
        Task<IEnumerable<Models.Contact>> GetAllContactsAsync(Models.ContactSearchDto model);
        Task<Models.Contact> GetContactDetailsAsync(Guid contactId, Guid userId);
        Task<IEnumerable<Models.Contact>> GetFavouritesAsync(Guid userId);
    }
}
