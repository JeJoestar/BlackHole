using Microsoft.EntityFrameworkCore;

namespace BlackHole.DAL
{
    public interface IDataContext : IDisposable
    {
        DbSet<User> Users { get; set; }

        DbSet<TEntity> Set<TEntity>()
            where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}