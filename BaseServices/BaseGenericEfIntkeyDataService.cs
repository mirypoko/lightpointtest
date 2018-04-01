using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Core.Base;
using Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.Base;

namespace BaseServices
{
    public abstract class BaseGenericEfIntKeyDataService<TEntity> : BaseGenericEfDataService<TEntity>, IBaseGenericDataService<int, TEntity> where TEntity : class, IBaseEntity<int>
    {
        protected BaseGenericEfIntKeyDataService(IGenericUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public virtual Task<List<TEntity>> GetListAsync(int? count = null, int? offset = null)
        {
            var query = Entities.Skip(offset.GetValueOrDefault());

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            var result = query.OrderBy(e => e.Id).ToListAsync();
            return result;
        }

        public virtual Task<TEntity> GetByIdOrDefaultAsync(int id)
        {
            return Entities.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
