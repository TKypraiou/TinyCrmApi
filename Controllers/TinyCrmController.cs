using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyCrm.Core.Model.Options;
using TinyCrmApi;
using TinyCrmApi.Data;
using TinyCrmApi.Model;
using TinyCrmApi.Options;
using TinyCrmApi.Services;

namespace TcrmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TinyCrmController : ControllerBase
    {

        private ICustomerService custService;
        private IProductService prodServ;
        private readonly ILogger<TinyCrmController> _logger;

        public TinyCrmController(ILogger<TinyCrmController> logger)
        {
            _logger = logger;
            custService = new CustomerService(new TinyCrmDbContext());
            prodServ = new ProductService(new TinyCrmDbContext());
        }

        [HttpGet]
        public String GetInfo()
        {
            return "TinyCrmApi";
        }

        [HttpGet("customers")]
        public ApiResult<List<Customer>> GetCustomers()
        {
            return custService.GetCustomers(20);
        }

        [HttpGet("customer/{id}")]
        public ApiResult<Customer> GetCustomer(int id)
        {
            return custService.GetCustomerById(id);
        }

        [HttpPost("customer")]
        public ApiResult<Customer> CreateCustomer(
            [FromBody] CreateCustomerOptions options)
        {
            return custService.Create(options);
        }

        [HttpPut("customer/{id}")]
        public ApiResult<Customer> UpdateCustomer([FromRoute] int id,
            [FromBody] CreateCustomerOptions options)
        {
            return custService.Update(id, options);
        }

        [HttpGet("customer")]
        public ApiResult<IQueryable<Customer>> getCustomerby(
             [FromQuery] int id, [FromQuery] string vat,
             [FromQuery]string email, [FromQuery] string name)
        {
            SearchCustomerOptions options = new SearchCustomerOptions
            {
                Email = email,
                FirstName = name,
                VatNumber = vat,
                Id = id
            };
            return custService.Search(options);
        }

        [HttpPost("customer/search")]
        public ApiResult<IQueryable<Customer>> getCustomerby2(
            [FromBody] SearchCustomerOptions options)
        {

            return custService.Search(options);
        }

        [HttpPost("product")]
        public ApiResult<Product> AddProduct(AddProductOptions options)
        {
            return prodServ.AddProduct(options);
        }

        [HttpPut("product/{productId}")]
        public bool UpdateProduct(Guid productId, UpdateProductOptions options)
        {
            return prodServ.UpdateProduct(productId, options);
        }

        [HttpGet("product/{productId}")]
        public ApiResult<Product> GetProductById(Guid productId)
        {
            return prodServ.GetProductById(productId);
        }

        [HttpGet("productStock")]
        public int SumOfStocks()
        {
            return prodServ.SumOfStocks();
        }

        [HttpPost("SearchProduct")]
        public IQueryable<Product> SearchProduct(SearchProductOptions options)
        {
            return prodServ.SearchProduct(options);
        }
    }
}
