using Application.Web.Models;

namespace Application.Web.Services.Interfaces
{
    public interface IProductService
    {
        public Task<ApiResponse> GetAllAsync();
        public Task<ApiResponse> GetByIdAsync(int productId);
        public Task<ApiResponse> CreateAsync(ProductRequest product);
        public Task<ApiResponse> UpdateAsync(ProductResponse product);
        public Task<ApiResponse> DeleteByIdAsync(int productId);
    }
}
