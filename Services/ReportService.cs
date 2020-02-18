using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCrmApi.Data;
using TinyCrmApi.Model;
using TinyCrmApi.Options;

namespace TinyCrmApi.Services
{
    public class ReportService : IReportService
    {
        private readonly TinyCrmDbContext context_;
        private readonly ICustomerService customer_;
        private readonly IProductService product_;
        private readonly IOrderService order_;

        public ReportService(
            TinyCrmDbContext context,
            ICustomerService customer,
            IProductService product,
            IOrderService order)
        {
            context_ = context;
            customer_ = customer;
            product_ = product;
            order_ = order;
        }

        public List<Customer> TopTenCustomersByGross()
        {
            var query = context_.Set<Customer>()
                       .AsQueryable();
            var top10 = query
                 .OrderByDescending(c => c.TotalGross)
                 .Take(10);
            return top10.ToList();
        }

        public ApiResult<int> OrdersInEachStatus(
                 SearchOrderOptions option)
        {
            if (option == null)
            {
                return new ApiResult<int>(
                        StatusCode.BadRequest,
                        "null options");
            }

            var status = option.OrderStatus;
            var result = order_.SearchOrder(option);
            var order = result.Data;
            var totalOrders = order.Count();
            return ApiResult<int>.CreateSuccessful(
                 totalOrders);
        }
    }
}
