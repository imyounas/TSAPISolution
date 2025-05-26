using System.Net.Http.Json;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TSWebAPI.Dtos;
using Xunit;

namespace TSIntegration.Test
{
    public class TSAPIIntegrationTests : IClassFixture<WebApplicationFactory<TSWebAPI.Program>>
    {
        private readonly WebApplicationFactory<TSWebAPI.Program> _factory;
        private readonly HttpClient _client;

        public TSAPIIntegrationTests(WebApplicationFactory<TSWebAPI.Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsSuccessAndProducts()
        {
            var response = await _client.GetAsync("/api/TSProduct");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(json);
            products.Should().NotBeNull();
            products.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetProductsByFilter_ReturnsFilteredProducts()        {

            var response = await _client.GetAsync("/api/TSProduct/filter?name=apple&pageNo=1&pageSize=3");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(json);
            products.Should().NotBeNull();
            products?.Count.Should().Be(3);
        }


        [Fact]
        public async Task GetProductsByFilter_WithEmptyName_ReturnsFilteredProducts()
        {

            var response = await _client.GetAsync("/api/TSProduct/filter?name=&pageNo=1&pageSize=7");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductDto>>(json);
            products.Should().NotBeNull();
            products?.Count.Should().Be(7);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedProduct()
        {
            var newProduct = new CreateProductRequestDto
            {
                Name = "IY_Desktop",
                Data = new Dictionary<string, object> { { "color", "Red" }, 
                    { "year", 2019 }, { "price", 1849.99 } , { "CPU model", "Intel Core i9" },
                    { "Hard disk size", "1 TB" } }
  
            };
            var content = new StringContent(JsonConvert.SerializeObject(newProduct), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/TSProduct", content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductDto>(json);
            product.Should().NotBeNull();
            product?.Name.Should().Be("IY_Desktop");     

        }

        [Fact]
        public async Task DeleteProduct_ReturnsSuccess()
        {
            // First, create a product to ensure it exists
            var newProduct = new CreateProductRequestDto
            {
                Name = "ProductToDelete",
                Data = new Dictionary<string, object> { { "color", "Blue" } }
            };
            var createContent = new StringContent(JsonConvert.SerializeObject(newProduct), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/TSProduct", createContent);
            createResponse.EnsureSuccessStatusCode();
            var createdJson = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<ProductDto>(createdJson);
            createdProduct.Should().NotBeNull();

            // Now, delete the product
            var deleteResponse = await _client.DeleteAsync($"/api/TSProduct?productid={createdProduct?.Id}");
            deleteResponse.EnsureSuccessStatusCode();
            var deleteJson = await deleteResponse.Content.ReadAsStringAsync();
            bool deleted = JsonConvert.DeserializeObject<bool>(deleteJson);
            deleted.Should().BeTrue();

        }
    }
}