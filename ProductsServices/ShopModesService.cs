using System;
using System.Collections.Generic;
using System.Text;
using BaseServices;
using Domain.DataBaseModels.Goods;
using Domain.DataBaseModels.Products;
using Domain.Services.Interfaces;
using Servises.Interfaces.Products;

namespace ProductsServices
{
    public class ShopModesService : BaseGenericEfIntKeyDataService<ShopMode>, IShopModesService
    {
        public ShopModesService(IGenericUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
