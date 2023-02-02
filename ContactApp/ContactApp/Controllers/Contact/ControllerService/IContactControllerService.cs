using ContactApp.Api.Controllers.Contact.Models;

namespace ContactApp.Api.Controllers.Contact.ControllerService
{
    public interface IContactControllerService
    {
        Task CreateContactAsync(ContactDto model, Guid userId);
        Task<ContactApp.Models.Contact> EditContactAsync(ContactDto model, Guid userId);
        Task DeleteContactAsync(Guid contactId, Guid userId);
        Task<IEnumerable<ContactApp.Models.Contact>> GetAllContactsAsync(ContactSearchObject searchObject, Guid userId);
        Task<ContactApp.Models.Contact> GetContactDetailsAsync(Guid contactId, Guid userId);
        Task<IEnumerable<ContactApp.Models.Contact>> GetFavouritesAsync(Guid userId);
    }
}
