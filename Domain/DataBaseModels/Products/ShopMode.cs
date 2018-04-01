using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Domain.Core.Base;

namespace Domain.DataBaseModels.Goods
{
    public class ShopMode: BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
