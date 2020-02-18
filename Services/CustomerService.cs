using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCrmApi.Data;
using TinyCrmApi.Model;
using TinyCrmApi.Options;

namespace TinyCrmApi.Services
{
    public class CustomerService : ICustomerService
    {
        private TinyCrmDbContext context;

        public CustomerService(TinyCrmDbContext dbContext)
        {
            context = dbContext;
        }

        public decimal TotalGross(int customerid)
        {
            var result = GetCustomerById(customerid);
            var orders = result.Data.Orders;

            if (orders == null)
            {
                result.Data.TotalGross = 0;
                return result.Data.TotalGross;
            }
            result.Data.TotalGross = orders.Sum(o => o.TotalCost);
            return result.Data.TotalGross;
        }

        public ApiResult<List<Customer>> GetCustomers(int howMany)
        {
            return
                ApiResult<List<Customer>>.CreateSuccessful(
                context.Set<Customer>()
                .Take(howMany)
                .ToList()
                );

        }

        public ApiResult<Customer> GetCustomerById(
            int customerId)
        {
            var customer = Search(new SearchCustomerOptions()
                {
                    Id = customerId
                }).Data;

            if (customer == null)
            {
                return new ApiResult<Customer>(
                    StatusCode.NotFound, $"Customer {customerId} not found");
            }

            return new ApiResult<Customer>()
            {
                ErrorCode = StatusCode.Success,
                Data = customer.SingleOrDefault()
            };
        }

        public ApiResult<IQueryable<Customer>> Search(
            SearchCustomerOptions options)
        {
            if (options == null)
            {
                return ApiResult<IQueryable<Customer>>.CreateUnsuccessful
                    (StatusCode.BadRequest, "null options");
            }

            if (string.IsNullOrWhiteSpace(options.Email) &&
                (options.Id == null || options.Id == 0) &&
                string.IsNullOrWhiteSpace(options.VatNumber) &&
                string.IsNullOrWhiteSpace(options.FirstName)
                )
            {
                return ApiResult<IQueryable<Customer>>.CreateUnsuccessful
                     (StatusCode.BadRequest, "null options");
            }

            var query = context
                .Set<Customer>()
                .AsQueryable();

            if (options.Id != null && options.Id != 0)
            {
                query = query.Where(
                    c => c.Id == options.Id);
            }

            if (options.VatNumber != null)
            {
                query = query.Where(
                    c => c.VatNumber == options.VatNumber);
            }

            if (options.Email != null)
            {
                query = query.Where(
                    c => c.Email.Equals(options.Email));
            }

            if (!string.IsNullOrWhiteSpace(options.FirstName))
            {
                query = query
                      .Where(c => c.FirstName.Contains(options.FirstName));
            }

            return ApiResult<IQueryable<Customer>>.CreateSuccessful(query);
        }

        public ApiResult<Customer> Create(CreateCustomerOptions options)
        {
            if (options == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(options.Email) ||
                !options.Email.Contains("@") ||
                string.IsNullOrWhiteSpace(options.VatNumber))
            {
                return null;
            }

            var customer = new Customer();
            customer.VatNumber = options.VatNumber;
            customer.Email = options.Email;
            customer.FirstName = options.FirstName;

            context.Set<Customer>().Add(customer);
            context.SaveChanges();

            return ApiResult<Customer>.CreateSuccessful(customer);
        }

        public ApiResult<Customer> Update(int id,
            CreateCustomerOptions options)
        {

            ApiResult<Customer> res = GetCustomerById(id);
            if (!res.Success) return res;

            if (options == null)
            {
                return ApiResult<Customer>.CreateUnsuccessful
                    (StatusCode.BadRequest, "null options");
            }
            Customer c = res.Data;
            if (!string.IsNullOrWhiteSpace(options.Email) &&
                options.Email.Contains("@"))
            {
                c.Email = options.Email;
            }

            context.SaveChanges();

            return ApiResult<Customer>.CreateSuccessful(c);
        }
    }
}
