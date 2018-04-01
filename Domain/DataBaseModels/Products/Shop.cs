using System.ComponentModel.DataAnnotations;
using Domain.Core.Base;
using Domain.DataBaseModels.Goods;

namespace Domain.DataBaseModels.Products
{
    public class Shop: BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int ShopModeId { get; set; }
        public ShopMode ShopMode { get; set; }
    }
}
