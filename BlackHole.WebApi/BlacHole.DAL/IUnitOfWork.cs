namespace BlackHole.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        public IDataRepository<User> Users { get; }

        public void Complete();

        public Task CompleteAsync();
    }
}