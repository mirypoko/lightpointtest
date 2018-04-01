using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Core.Base;

namespace Domain.DataBaseModels.Products
{
    public class Product: BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        [Url]
        public string ImgUrl { get; set; }
    }
}
