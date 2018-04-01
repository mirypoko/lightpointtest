using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseServices;
using Domain.DataBaseModels.Products;
using Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Servises.Interfaces.Products;

namespace ProductsServices
{
    public class ShopsService : BaseGenericEfIntKeyDataService<Shop>, IShopsService
    {
        public ShopsService(IGenericUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override Task<List<Shop>> GetListAsync(int? count = null, int? offset = null)
        {
            var query = Entities.Include(e => e.ShopMode).Skip(offset.GetValueOrDefault());

            if (count != null)
            {
                query = query.Take(count.Value);
            }

            var result = query.OrderBy(e => e.Id).ToListAsync();
            return result;
        }

        public override Task<Shop> GetByIdOrDefaultAsync(int id)
        {
            return Entities.Include(e => e.ShopMode).FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
