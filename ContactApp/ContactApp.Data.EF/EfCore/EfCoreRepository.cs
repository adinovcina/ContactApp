using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace ContactApp.Data.EF.EfCore
{
    public class EfCoreRepository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationContext _context;

        public EfCoreRepository(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "", int skip = 0, int take = 20)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                return await orderBy(query).Skip(skip).Take(take).ToListAsync();

            return await query.Skip(skip).Take(take).ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Trying to insert null entity - entity can't be null");
            }

            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException("Trying to update with null entity - updated entity can't be null");
            }

            EntityEntry<T>? trackedEntity = _context.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                IEntityType? entityType = _context.Model.FindEntityType(typeof(T));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(T).Name} is not part of EF Core DbContext model");
                }

                string? primaryKeyName = entityType.FindPrimaryKey()!.Properties.Select(p => p.Name).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(primaryKeyName))
                {
                    Type primaryKeyType = entityType.FindPrimaryKey()!.Properties.Select(p => p.ClrType).FirstOrDefault()!;

                    object? primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    object? primaryValue = entity.GetType().GetProperty(primaryKeyName)!.GetValue(entity, null);

                    if (primaryKeyDefaultValue != null && primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                _context.Set<T>().Update(entity);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
