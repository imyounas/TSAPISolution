using TSWebAPI.Dtos;

namespace TSWebAPI.Interfaces
{
    public interface IExternalAPIService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductRequestDto product);
        Task<bool> DeleteProductAsync(DeleteProductRequestDto delProduct);
    }
}
