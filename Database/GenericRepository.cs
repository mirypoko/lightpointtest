using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.EntityFrameworkCore
{
    public class GenericRepository<TEntity> : EntityFrameworkAsyncQueryable<TEntity>, IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(DbSet<TEntity> dbSet) : base(dbSet)
        {

        }

        public Task AddAsync(TEntity entity)
            => Entities.AddAsync(entity);

        public Task AddRangeAsync(params TEntity[] entities)
        {
            return Entities.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            Entities.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        public void RemoveRange(params TEntity[] entities)
        {
            Entities.RemoveRange(entities);
        }

        public void Load()
        {
            Entities.Load();
        }
    }
}
