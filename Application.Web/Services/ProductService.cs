using Application.Web.Models;
using Application.Web.Services.Interfaces;

namespace Application.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        private readonly IConfiguration _config;
        private readonly string baseUrl;
        public ProductService(IBaseService baseService, IConfiguration config)
        {
            _baseService = baseService;
            _config = config;
            baseUrl = _config["ServiceUrls:ProductApi"] ?? string.Empty;
        }
        public async Task<ApiResponse> CreateAsync(ProductRequest product)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.POST,
                ApiUrl = $"{baseUrl}/api/product",
                Data = product
            });
        }

        public async Task<ApiResponse> DeleteByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.DELETE,
                ApiUrl = $"{baseUrl}/api/product/{productId}"
            });
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.GET,
                ApiUrl = $"{baseUrl}/api/product"
            });
        }

        public async Task<ApiResponse> GetByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.GET,
                ApiUrl = $"{baseUrl}/api/product/{productId}"
            });
        }

        public async Task<ApiResponse> UpdateAsync(ProductResponse product)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.PUT,
                ApiUrl = $"{baseUrl}/api/product/{product.ProductId}",
                Data = product
            });
        }
    }
}
