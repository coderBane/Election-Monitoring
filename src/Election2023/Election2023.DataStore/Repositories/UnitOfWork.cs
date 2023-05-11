using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.Interfaces.Services;
using Election2023.DataStore.Database;

using System.Collections;
using Microsoft.Extensions.Logging;

namespace Election2023.DataStore.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
        private readonly ICurrentUserService _currentUserService;
        private readonly ElectionDbContext _dbContext;
        private readonly ILogger _logger;
        private Hashtable? _repositories;
        private bool _disposed;

		public UnitOfWork(IDbContextFactory<ElectionDbContext> dbContext, ICurrentUserService currentUserService, ILoggerFactory loggerFactory)
		{
            _dbContext = dbContext.CreateDbContext() ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = loggerFactory.CreateLogger("UoF_logs");
            _currentUserService = currentUserService;
		}

        public IRepository<T> Repository<T>() where T : class
        {
            _repositories ??= new();

            var type = typeof(T).Name;

            if(!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Repository<T>(_dbContext, _logger);
                _logger.LogDebug("Added new repository instance of type " + type);
            }

            return (IRepository<T>)_repositories[type]!;
        }

        public async Task<int> CommitAsync()
        {
            _logger.LogDebug("Saving changes to Database...");
            return await _dbContext.SaveChangesAsync();
        }

        public Task RollBack()
        {
            _logger.LogDebug("Rolling back changes...");
            _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                {
                    //_repositories?.Clear();
                    _dbContext.Dispose();
                    _logger.LogDebug("Disposed of database context");
                }

            _disposed = true;
        }
    }
}

