using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyCrmApi.Options
{
    public class SearchOrderOptions
    {
        public int? CustomerId { get; set; }
        public Guid? OrderId { get; set; }
        public string VatNumber { get; set; }
        public Status OrderStatus { get; set; }
    }
}
