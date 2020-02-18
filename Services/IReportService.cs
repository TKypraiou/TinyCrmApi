using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCrmApi.Model;
using TinyCrmApi.Options;

namespace TinyCrmApi.Services
{
    interface IReportService
    {
        public List<Customer> TopTenCustomersByGross();
        public ApiResult<int> OrdersInEachStatus(SearchOrderOptions option);
    }
}
