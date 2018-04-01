using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.Core.Base;
using Domain.DataBaseModels;
using Domain.DataBaseModels.Goods;
using Domain.DataBaseModels.Products;
using Servises.Interfaces.Base;

namespace Servises.Interfaces.Products
{
    public interface IProductsService : IBaseGenericDataService<int, Product>
    {
        Task<int> CountAsync(string searchString, int? shopId);

        Task<List<Product>> SearchAsync(int? count, int? offset, string searchString, int? shopId);
    }
}
