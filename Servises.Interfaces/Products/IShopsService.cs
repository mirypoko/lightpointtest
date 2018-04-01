using Domain.DataBaseModels.Products;
using Servises.Interfaces.Base;

namespace Servises.Interfaces.Products
{
    public interface IShopsService: IBaseGenericDataService<int, Shop>
    {
    }
}
