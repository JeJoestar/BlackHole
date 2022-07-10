using System.Linq.Expressions;

namespace BlackHole.DAL
{
    public interface IDataRepository<TEntity>
    {
        Task<List<TEntity>> GetByFilter(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> GetAll();

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task Delete(TEntity entity);
    }
}