using AutoMapper;
using ContactApp.Data.EF.EfCore;
using ContactApp.Data.EF.Helpers;
using System.Linq.Expressions;

namespace ContactApp.Data.EF.Repositories.Contact
{
    public class ContactRepository : EfCoreRepository<Entities.Contact>, IContactRepository
    {
        private readonly IMapper _mapper;

        public ContactRepository(IMapper mapper, ApplicationContext context) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<Models.Contact>> GetAllContactsAsync(Models.ContactSearchDto model)
        {
            var predicate = MakeBasePredicate(model);
            var contacts = await GetAllAsync(predicate, null, "", model.Skip ?? 0, model.Take ?? 20);
            return _mapper.Map<IEnumerable<Models.Contact>>(contacts);
        }

        public async Task<IEnumerable<Models.Contact>> GetFavouritesAsync(Guid userId)
        {
            var favourites = await GetAllAsync(x => x.UserId.Equals(userId) && x.IsFavourite == true);
            return _mapper.Map<IEnumerable<Models.Contact>>(favourites);
        }

        public async Task<Models.Contact> GetContactDetailsAsync(Guid contactId, Guid userId)
        {
            var entity = await FindContactAndCheckIfUserIsOwner(contactId, userId);
            return _mapper.Map(entity, new Models.Contact());
        }

        public async Task CreateContactAsync(Models.Contact model)
        {
            var entity = _mapper.Map<Entities.Contact>(model);
            await InsertAsync(entity);
        }

        public async Task<Models.Contact> EditContactAsync(Models.Contact model)
        {
            var entity = await FindContactAndCheckIfUserIsOwner(model.Id!.Value, model.UserId);

            _mapper.Map(model, entity);
            await UpdateAsync(entity);
            return model;
        }

        public async Task DeleteContactAsync(Guid contactId, Guid userId)
        {
            var entity = await FindContactAndCheckIfUserIsOwner(contactId, userId);
            _context.Contacts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        #region Private Methods

        private static bool VerifyIfUserIsOwnerOfContact(Guid userId, Guid ownerId)
        {
            return ownerId.Equals(userId);
        }

        private async Task<Entities.Contact> FindContactAndCheckIfUserIsOwner(Guid contactId, Guid userId)
        {
            var entity = await _context.Contacts.FindAsync(contactId);
            if (entity == null)
            {
                throw new KeyNotFoundException("Entity not found");
            }

            var isOwner = VerifyIfUserIsOwnerOfContact(userId, entity.UserId);
            if (isOwner == false)
            {
                throw new UnauthorizedAccessException("User is not owner of contact");
            }

            return entity;
        }

        private static Expression<Func<Entities.Contact, bool>> MakeBasePredicate(Models.ContactSearchDto model)
        {
            Expression<Func<Entities.Contact, bool>> predicate;

            if (!string.IsNullOrWhiteSpace(model.SearchPhrase))
            {
                predicate = PredicateBuilder.False<Entities.Contact>();
                predicate = predicate.Or(x => x.PhoneNumber!.ToLower().Contains(model.SearchPhrase.ToLower()));
                predicate = predicate.Or(x => x.Name!.ToLower().Contains(model.SearchPhrase.ToLower()));
                predicate = predicate.Or(x => x.Address!.ToLower().Contains(model.SearchPhrase.ToLower()));
                predicate = predicate.Or(x => x.Email!.ToLower().Contains(model.SearchPhrase.ToLower()));
            }
            else
            {
                predicate = PredicateBuilder.True<Entities.Contact>();
            }

            predicate = predicate.And(x => x.UserId.Equals(model.UserId));

            return predicate;
        }

        #endregion
    }
}
