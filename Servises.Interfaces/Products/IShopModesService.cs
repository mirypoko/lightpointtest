using System;
using System.Collections.Generic;
using System.Text;
using Domain.DataBaseModels.Goods;
using Domain.DataBaseModels.Products;
using Servises.Interfaces.Base;

namespace Servises.Interfaces.Products
{
    public interface IShopModesService : IBaseGenericDataService<int, ShopMode>
    {
    }
}
