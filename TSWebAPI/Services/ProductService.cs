using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TSWebAPI.Controllers;
using TSWebAPI.Dtos;
using TSWebAPI.Dtos.Validators;
using TSWebAPI.Interfaces;

namespace TSWebAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IExternalAPIService _externalAPIService;
        private readonly IValidator<ProductFilterRequestDto> _productFilterValidator;
        private readonly IValidator<CreateProductRequestDto> _createProductValidator;
        private readonly IValidator<DeleteProductRequestDto> _deleteProductValidator;
        private readonly ILogger<ProductService> _logger;
        public ProductService(ILogger<ProductService> logger, IExternalAPIService externalAPIService, 
            IValidator<ProductFilterRequestDto> productFilterValidator,
            IValidator<CreateProductRequestDto> createProductValidator, 
            IValidator<DeleteProductRequestDto> deleteProductValidator)
        {
            _logger = logger;
            _externalAPIService = externalAPIService;
            _productFilterValidator = productFilterValidator;
            _createProductValidator = createProductValidator;
            _deleteProductValidator = deleteProductValidator;
            _deleteProductValidator = deleteProductValidator;
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _externalAPIService.GetProductsAsync();
                if (products == null || !products.Any())
                {
                    return Result<IEnumerable<ProductDto>>.NotFound();
                }

                return Result<IEnumerable<ProductDto>>.Success(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products from external API.");
                return Result<IEnumerable<ProductDto>>.Error($"An error occurred while fetching products. Error: [{ex.Message}]");
            }
           
        }

        public async Task<Result<IEnumerable<ProductDto>>> GetProductByNameAsync(ProductFilterRequestDto productFilter)
        {
            try
            {
                var validation = await _productFilterValidator.ValidateAsync(productFilter);

                if (!validation.IsValid)
                {
                    _logger.LogDebug("Validation failed for product filter: {@ProductFilter}", productFilter);
                    return Result<IEnumerable<ProductDto>>.Invalid(validation.AsErrors());
                }

                var products = await _externalAPIService.GetProductsAsync();

                IEnumerable<ProductDto>? filteredProducts = null;
                if (products != null && products.Any())
                {
                    
                      filteredProducts = products
                            // if productFilter.Name is null or empty, return all products                            
                        .Where(p => string.IsNullOrEmpty(productFilter.Name) || p.Name.Contains(productFilter.Name, StringComparison.OrdinalIgnoreCase) )
                            .Skip((productFilter.PageNo - 1) * productFilter.PageSize)
                            .Take(productFilter.PageSize);
                                            
                }

                if (filteredProducts == null || !filteredProducts.Any())
                {
                    _logger.LogDebug("No products found matching the filter criteria: {@ProductFilter}", productFilter);
                    return Result<IEnumerable<ProductDto>>.NotFound();
                }

                _logger.LogDebug("Filtered products: {@FilteredProducts}", filteredProducts);
                return Result<IEnumerable<ProductDto>>.Success(filteredProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products by name filter from external API.");
                return Result<IEnumerable<ProductDto>>.Error($"An error occurred while fetching products by filter. Error: [{ex.Message}]");
            }
        }

        public async Task<Result<ProductDto>> CreateProductAsync(CreateProductRequestDto product)
        {
            try
            {
                var validation = await _createProductValidator.ValidateAsync(product);

                if (!validation.IsValid)
                {
                    _logger.LogDebug("Validation failed for product creation: {@Product}", product);
                    return Result<ProductDto>.Invalid(validation.AsErrors());
                }

                var newProduct = await _externalAPIService.CreateProductAsync(product);

                _logger.LogDebug("New product created with Id {NewProductId}", newProduct.Id);
                return Result<ProductDto>.Success(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product.");
                return Result<ProductDto>.Error($"An error occurred while creating product. Error: [{ex.Message}]");
            }
        }

        public async Task<Result<bool>> DeleteProductAsync(DeleteProductRequestDto dto)
        {
            try
            {
                var validation = await _deleteProductValidator.ValidateAsync(dto);

                if (!validation.IsValid)
                {
                    _logger.LogDebug("Validation failed for product deletion: {@DeleteProduct}", dto);
                    return Result<bool>.Invalid(validation.AsErrors());
                }

                var result = await _externalAPIService.DeleteProductAsync(dto);
                _logger.LogDebug("Product successfully deleted with Id {deletedProductId}", dto.Id);
                return Result<bool>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleteing the product.");
                return Result<bool>.Error($"An error occurred while deleteing product. Error: [{ex.Message}]");
            }
        }

        
    }
}
