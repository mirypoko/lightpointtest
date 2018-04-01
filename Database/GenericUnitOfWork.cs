using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Services.Interfaces;

namespace Database.EntityFrameworkCore
{
    public sealed class GenericUnitOfWork : IGenericUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private ApplicationDbContext _dbContext;

        public GenericUnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            Type entityType = typeof(TEntity);
            if (!_repositories.ContainsKey(entityType))
            {
                var repository = new GenericRepository<TEntity>(_dbContext.Set<TEntity>());
                _repositories.Add(entityType, repository);
            }

            return (IGenericRepository<TEntity>)_repositories[entityType];
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }
    }
}
