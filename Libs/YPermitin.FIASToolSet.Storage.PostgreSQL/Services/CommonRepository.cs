using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using YPermitin.FIASToolSet.Storage.PostgreSQL.DbContexts;

namespace YPermitin.FIASToolSet.Storage.PostgreSQL.Services
{
    public abstract class CommonRepository
    {
        private static HashSet<EntityState> _entityStatesToSaveChanges = new()
        {
            EntityState.Added,
            EntityState.Modified,
            EntityState.Deleted
        };
        
        protected readonly BulkConfig BulkConfigDefault = new BulkConfig()
        {
            BatchSize = 10000,
            WithHoldlock = true,
            UseTempDB = true
        };
        
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
        
        public async Task<bool> SaveBulkAsync()
        {
            var entities = _context.ChangeTracker
                .Entries()
                .Where(e => _entityStatesToSaveChanges.Contains(e.State) )
                .Select(e => e.Entity)
                .ToList();

            var changesExists = (entities.Count() >= 0);
            
            await _context.BulkSaveChangesAsync(BulkConfigDefault);
            
            if (changesExists)
            {
                _context.ChangeTracker.Clear();
            }

            return changesExists;
        }
        
        public async Task SaveWithIdentityInsertAsync<T>()
        {
            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                await SaveAsync();
                await transaction.CommitAsync();
            }
        }

        public void ClearChangeTracking()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
