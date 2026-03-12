using Newtonsoft.Json;
using Services.ShoppingCartApi.Models.Dto;
using Services.ShoppingCartApi.Services.Interfaces;
using System.Text.Json.Serialization;

namespace Services.ShoppingCartApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("ProductApi");
            var response = await client.GetAsync("api/product");
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto != null && responseDto.IsSuccess && responseDto.Data != null)
            {
                var dataString = responseDto.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    var products = JsonConvert.DeserializeObject<List<ProductDto>>(dataString);
                    return products ?? new List<ProductDto>();
                }
            }
            return new List<ProductDto>();
        }
    }
}
