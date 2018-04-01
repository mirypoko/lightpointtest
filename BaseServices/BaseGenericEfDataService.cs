using System;
using System.Threading.Tasks;
using Domain.Core;
using Domain.Services.Interfaces;
using Utils;

namespace BaseServices
{
    public abstract class BaseGenericEfDataService<TEntity> where TEntity : class 
    {
        protected IGenericUnitOfWork UnitOfWork;

        protected IGenericRepository<TEntity> Entities { get; }

        protected BaseGenericEfDataService(IGenericUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            Entities = unitOfWork.GetRepository<TEntity>();
        }

        public virtual Task<int> CountAsync()
        {
            return Entities.CountAsync();
        }

        public virtual async Task<ServiceResult> CreateAsync(TEntity entity)
        {
            var errors = AttributeValidator.Validation(entity);
            if (errors != null)
            {
                return new ServiceResult(false, errors);
            }

            try
            {
                await Entities.AddAsync(entity);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }

            return new ServiceResult(true);
        }

        public virtual async Task<ServiceResult> DeleteAsync(TEntity entity)
        {
            try
            {
                Entities.Remove(entity);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }
            return new ServiceResult(true);
        }

        public virtual async Task<ServiceResult> UpdateAsync(TEntity entity)
        {
            var errors = AttributeValidator.Validation(entity);
            if (errors != null)
            {
                return new ServiceResult(false, errors);
            }

            try
            {
                Entities.Update(entity);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceResult(false, ex.Message);
            }

            return new ServiceResult(true);
        }
    }
}
