using ContactApp.Data.EF.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactApp.Data.EF.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        #region Public Properties

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public ICollection<Contact>? PersonalContacts { get; set; }

        #endregion
    }
}
