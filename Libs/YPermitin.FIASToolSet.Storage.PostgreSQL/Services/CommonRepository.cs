using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Services
{
    public abstract class CommonRepository
    {
        // ReSharper disable once InconsistentNaming
        protected readonly FIASToolSetServiceContext _context;

        protected CommonRepository(FIASToolSetServiceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> BeginTransactionAsync()
        {
            bool transactionStarted = false;
            if (_context.Database.CurrentTransaction == null)
            {
                await _context.Database.BeginTransactionAsync();
                transactionStarted = true;
            }

            return transactionStarted;
        }
        
        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
