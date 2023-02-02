using System.Linq.Expressions;

namespace ContactApp.Data.EF.EfCore
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "", int skip = 0, int take = 20);

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);
    }
}
