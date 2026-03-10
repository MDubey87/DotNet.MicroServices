using Services.ProductApi.Models.Dto;

namespace Services.ProductApi.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductDto>> GetAll();
        public Task<ProductDto?> GetById(int productId);
        public Task<ProductDto> Create(RequestDto coupon);
        public Task<bool?> Update(RequestDto coupon, int productId);
        public Task<bool?> DeleteById(int productId);
    }
}
