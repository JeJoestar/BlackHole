using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackHole.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataContext _context;

        private IDataRepository<User> _chef;


        public IDataRepository<User> Users
            => _chef ??= new DataRepository<User>(_context);

        private bool _disposed;

        public UnitOfWork(IDataContext context)
        {
            _context = context;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
