using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyCrmApi.Options
{
    public class UpdateProductOptions
    {
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public string Description { get; set; }
    }
}
