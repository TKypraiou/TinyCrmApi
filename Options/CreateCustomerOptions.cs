using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyCrmApi.Options
{
    public class CreateCustomerOptions
    {
        public string VatNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
    }
}
