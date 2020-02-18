using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCrmApi.Model;
using TinyCrmApi.Options;

namespace TinyCrmApi.Services
{
    public interface IOrderService
    {
        public ApiResult<Order> CreateOrder(CreateOrderOptions options);
        public ApiResult<IQueryable<Order>> SearchOrder(SearchOrderOptions options);
    }
}
