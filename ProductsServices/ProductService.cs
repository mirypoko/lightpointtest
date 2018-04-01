using BaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Core;
using Domain.DataBaseModels;
using Domain.DataBaseModels.Products;
using Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.Products;

namespace ProductsServices
{
    public class ProductsService : BaseGenericEfIntKeyDataService<Product>, IProductsService
    {
        public ProductsService(IGenericUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<int> CountAsync(string searchString, int? shopId)
        {
            IQueryable<Product> qureble = Entities.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                qureble = qureble.Where(f => f.Name.Contains(searchString) || f.Description.Contains(searchString));
            }

            if (shopId != null)
            {
                qureble = qureble.Where(s => s.ShopId == shopId);
            }

            return await qureble.CountAsync();
        }

        public Task<List<Product>> SearchAsync(int? count, int? offset, string searchString, int? shopId)
        {
            IQueryable<Product> qureble = Entities.Include(e => e.Shop);
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                qureble = qureble.Where(f => f.Name.Contains(searchString) || f.Description.Contains(searchString));
            }

            if (shopId != null)
            {
                qureble = qureble.Where(s => s.ShopId == shopId);
            }


            qureble = qureble.Skip(offset.GetValueOrDefault());

            if (count == null)
            {
                return qureble.ToListAsync();
            }

            return qureble.Take(count.Value).ToListAsync();
        }

        public override Task<List<Product>> GetListAsync(int? count = null, int? offset = null)
        {
            var query = Entities.Include(e => e.Shop).Skip(offset.GetValueOrDefault());

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            var result = query.OrderBy(e => e.Id).ToListAsync();
            return result;
        }

        public override Task<Product> GetByIdOrDefaultAsync(int id)
        {
            return Entities.Include(e => e.Shop).FirstOrDefaultAsync(e => e.Id == id);
        }

        public override async Task<ServiceResult> UpdateAsync(Product entity)
        {
            if (await Entities.AnyAsync(e => e.Name == entity.Name))
            {
                return new ServiceResult(false, $"Name {entity.Name} already exist");
            }
            return await base.UpdateAsync(entity);
        }

        public override async Task<ServiceResult> CreateAsync(Product entity)
        {
            if (await Entities.AnyAsync(e => e.Name == entity.Name))
            {
                return new ServiceResult(false, $"Name {entity.Name} already exist");
            }
            return await base.CreateAsync(entity);
        }
    }
}
