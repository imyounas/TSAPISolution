using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Xml.Linq;
using TSWebAPI.Dtos;
using TSWebAPI.Interfaces;

namespace TSWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSProductController : ControllerBase
    {
        private readonly ILogger<TSProductController> _logger;
        private readonly IProductService _productService;
        public TSProductController(ILogger<TSProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        

        [HttpGet(Name = "GetAllProducts")]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.NotFound, ResultStatus.Invalid)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();           
            return this.ToActionResult(products);
        }

        [HttpGet("filter")]
        [ExpectedFailures(ResultStatus.Error , ResultStatus.NotFound, ResultStatus.Invalid)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByFilter( string? name, int pageNo, int pageSize)
        {
            var productFilter = new ProductFilterRequestDto
            {
                Name = name,
                PageNo = pageNo,
                PageSize = pageSize
            };
            var products = await _productService.GetProductByNameAsync(productFilter);
            return this.ToActionResult(products);
        }


        [HttpPost]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequestDto productDto)
        {

            var product = await _productService.CreateProductAsync(productDto);
            return this.ToActionResult(product);
        }

        [HttpDelete]
        [ExpectedFailures(ResultStatus.Error, ResultStatus.Invalid)]
        public async Task<ActionResult<bool>> DeleteProduct(string productid)
        {
            var delProduct = new DeleteProductRequestDto
            {
                Id = productid
            };
            var result = await _productService.DeleteProductAsync(delProduct);
            return this.ToActionResult(result);
        }
    }
}
