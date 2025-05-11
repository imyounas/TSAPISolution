using Ardalis.Result;
using TSWebAPI.Dtos;

namespace TSWebAPI.Interfaces
{
    public interface IProductService
    {
        // Define methods for product-related operations
        Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
        Task<Result<IEnumerable<ProductDto>>> GetProductByNameAsync(ProductFilterRequestDto productFilter);
        Task<Result<ProductDto>> CreateProductAsync(CreateProductRequestDto product);
        Task<Result<bool>> DeleteProductAsync(DeleteProductRequestDto delProduct);
    }
}
