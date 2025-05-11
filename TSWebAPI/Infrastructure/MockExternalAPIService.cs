using System.Net.Http;
using System;
using TSWebAPI.Dtos;
using TSWebAPI.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System.Text;
using TSWebAPI.Services;
using Microsoft.Extensions.Options;
using TSWebAPI.Common.Options;
using Newtonsoft.Json.Converters;

namespace TSWebAPI.Infrastructure
{
    public class MockExternalAPIService : IExternalAPIService
    {
        private readonly HttpClient _client;
        private readonly ILogger<MockExternalAPIService> _logger;
        private readonly MockAPIClientSettings _settings;


        public MockExternalAPIService(ILogger<MockExternalAPIService> logger, 
            HttpClient client, IOptions<MockAPIClientSettings> options)
        {
            _logger = logger;
            _client = client;
            _settings = options.Value;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var endpoint = _settings.ProductEndPoint;
            var response = await _client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                IEnumerable<ProductDto>? products = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(data);
                
                if (products is null)
                {
                    _logger.LogDebug("Deserialization of {@GetProductsJsonData} resulted in a null for IEnumerable<ProductDto>.", data);
                    throw new JsonException("Deserialization resulted in a null IEnumerable<ProductDto>.");
                }

                return products;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("MockAPIService Get Error: {HTTPCode} , {Error} ", response.StatusCode , errorContent);
            throw new HttpRequestException($"{errorContent}");

        }


        public async Task<ProductDto> CreateProductAsync(CreateProductRequestDto dto)
        {
            var endpoint = _settings.ProductEndPoint;

            var jsonPayload = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonPayload, Encoding.UTF8, _settings.AcceptHeader);

            var response = await _client.PostAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                ProductDto? newProduct = JsonConvert.DeserializeObject<ProductDto>(data);                 
                
                if (newProduct is null)
                {
                    _logger.LogDebug("Deserialization of {@CreateJsonData} resulted in a null for ProductDto.", data);
                    throw new JsonException("Deserialization resulted in a null ProductDto.");
                }

                return newProduct;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("MockAPIService Create Error: {HTTPCode} , {Error} ", response.StatusCode, errorContent);
            throw new HttpRequestException($"{errorContent}");
        }

        public async Task<bool> DeleteProductAsync(DeleteProductRequestDto dto)
        {
            var endpoint = $"{_settings.ProductEndPoint}/{dto.Id}";
            
            var response = await _client.DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("MockAPIService Delete Error: {HTTPCode} , {Error} ", response.StatusCode, errorContent);
            throw new HttpRequestException($"{errorContent}");
        }
    }
}
