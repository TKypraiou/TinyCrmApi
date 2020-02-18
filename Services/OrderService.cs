using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCrmApi.Data;
using TinyCrmApi.Model;
using TinyCrmApi.Options;

namespace TinyCrmApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly TinyCrmDbContext context_;
        private readonly ICustomerService customer_;
        private readonly IProductService product_;

        public OrderService(
            TinyCrmDbContext context,
            ICustomerService customers,
            IProductService products)
        {
            context_ = context;
            customer_ = customers;
            product_ = products;

        }

        public ApiResult<Order> CreateOrder(
            CreateOrderOptions options)
        {
            if (options == null)
            {
                return new ApiResult<Order>(
                    StatusCode.BadRequest, "null options");
            }

            var cresult = customer_
                .GetCustomerById(options.CustomerId);

            if (!cresult.Success)
            {
                return ApiResult<Order>.Create(cresult);
            }

            var order = new Order();

            foreach (var id in options.ProductIds)
            {

                var prodResult = product_
                     .GetProductById(id);

                if (!prodResult.Success)
                {

                    return ApiResult<Order>.Create(
                        prodResult);
                }

                order.TotalCost = 0;
                order.TotalCost += prodResult.Data.Price;

                order.OrderProducts.Add(
                    new OrderProduct()
                    {
                        Product = prodResult.Data
                    });
            }

            context_.Add(order);
            cresult.Data.Orders.Add(order);
            context_.SaveChanges();

            return ApiResult<Order>.CreateSuccessful(order);
        }

        public ApiResult<IQueryable<Order>> SearchOrder(
            SearchOrderOptions options)
        {
            if (options == null)
            {
                return ApiResult<IQueryable<Order>>.CreateUnsuccessful(
                    StatusCode.BadRequest, "Null order");
            }

            if (options.OrderId == null
                && options.CustomerId == null
                && options.VatNumber == null)
            {
                return new ApiResult<IQueryable<Order>>(
                    StatusCode.BadRequest, "Null order");
            }

            var query = context_
                .Set<Order>()
                .AsQueryable();

            if (options.OrderId != null)
            {//check guid
                query = query.Where(o => o.Id.Equals(options.OrderId));
            }

            if (options.CustomerId != null)
            {
                query = query.Where(o => o.CustomerId.Equals(options.CustomerId));
            }

            if (options.VatNumber != null)
            {
                query = query.Where(o => o.Customer.VatNumber.Equals(options.VatNumber));
            }

            return ApiResult<IQueryable<Order>>.CreateUnsuccessful(
                    StatusCode.BadRequest, "Null order");
        }

    }
}
