using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database.EntityFrameworkCore
{
    public class EntityFrameworkAsyncQueryable<TEntity> : IAsyncEnumerable<TEntity>, IAsyncQueryable<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> Entities;

        public readonly IQueryable<TEntity> Queryable;

        public Type ElementType => Queryable.ElementType;

        public Expression Expression => Queryable.Expression;

        public IQueryProvider Provider => Queryable.Provider;

        public EntityFrameworkAsyncQueryable(DbSet<TEntity> dbSet)
        {
            Entities = dbSet;
            Queryable = Entities.AsQueryable();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Entities.AsQueryable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IAsyncEnumerator<TEntity> IAsyncEnumerable<TEntity>.GetEnumerator()
        {
            return Entities.ToAsyncEnumerable().GetEnumerator();
        }

        public Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AllAsync(this, predicate, cancellationToken);
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AnyAsync(this, cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AnyAsync(this, predicate, cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<decimal> AverageAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<decimal?> AverageAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<double> AverageAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<double?> AverageAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<float> AverageAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<float?> AverageAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.AverageAsync(this, selector, cancellationToken);
        }

        public Task<bool> ContainsAsync(TEntity item, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ContainsAsync(this, item, cancellationToken);
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.CountAsync(this, predicate, cancellationToken);
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.CountAsync(this, cancellationToken);
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.FirstAsync(this, predicate, cancellationToken);
        }

        public Task<TEntity> FirstAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.FirstAsync(this, cancellationToken);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(this, predicate, cancellationToken);
        }

        public Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(this, cancellationToken);
        }

        public Task ForEachAsync(Action<TEntity> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ForEachAsync(this, action, cancellationToken);
        }

        public IQueryable<TEntity> Include(string navigationPropertyPath)
        {
            return EntityFrameworkQueryableExtensions.Include(this, navigationPropertyPath);
        }

        public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LastAsync(this, predicate, cancellationToken);
        }

        public Task<TEntity> LastAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LastAsync(this, cancellationToken);
        }

        public Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LastOrDefaultAsync(this, predicate, cancellationToken);
        }

        public Task<TEntity> LastOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LastOrDefaultAsync(this, cancellationToken);
        }

        public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LoadAsync(this, cancellationToken);
        }

        public Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LongCountAsync(this, cancellationToken);
        }

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.LongCountAsync(this, predicate, cancellationToken);
        }

        public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.MaxAsync(this, selector, cancellationToken);
        }

        public Task<TEntity> MaxAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.MaxAsync(this, cancellationToken);
        }

        public Task<TEntity> MinAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.MinAsync(this, cancellationToken);
        }

        public Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.MinAsync(this, selector, cancellationToken);
        }

        public Task<TEntity> SingleAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SingleAsync(this, cancellationToken);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SingleAsync(this, predicate, cancellationToken);
        }

        public Task<TEntity> SingleOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SingleAsync(this, cancellationToken);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SingleOrDefaultAsync(this, predicate, cancellationToken);
        }

        public Task<long> SumAsync(Expression<Func<TEntity, long>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<long?> SumAsync(Expression<Func<TEntity, long?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<double> SumAsync(Expression<Func<TEntity, double>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<double?> SumAsync(Expression<Func<TEntity, double?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<float> SumAsync(Expression<Func<TEntity, float>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<decimal?> SumAsync(Expression<Func<TEntity, decimal?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<float?> SumAsync(Expression<Func<TEntity, float?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<int> SumAsync(Expression<Func<TEntity, int>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<int?> SumAsync(Expression<Func<TEntity, int?>> selector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.SumAsync(this, selector, cancellationToken);
        }

        public Task<TEntity[]> ToArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ToArrayAsync(this, cancellationToken);
        }

        public Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey>(Func<TEntity, TKey> keySelector, CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ToDictionaryAsync(this, keySelector, cancellationToken);
        }

        public Task<Dictionary<TKey, TEntity>> ToDictionaryAsync<TKey>(Func<TEntity, TKey> keySelector, IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ToDictionaryAsync(this, keySelector, comparer, cancellationToken);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<TEntity, TKey> keySelector, Func<TEntity, TElement> elementSelector,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ToDictionaryAsync(this, keySelector, elementSelector, cancellationToken);
        }

        public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey, TElement>(Func<TEntity, TKey> keySelector, Func<TEntity, TElement> elementSelector, IEqualityComparer<TKey> comparer,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ToDictionaryAsync(this, keySelector, elementSelector, comparer, cancellationToken);
        }

        public Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return EntityFrameworkQueryableExtensions.ToListAsync(this, cancellationToken);
        }

        public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entities.Where(predicate).ToListAsync(cancellationToken);
        }
    }
}
