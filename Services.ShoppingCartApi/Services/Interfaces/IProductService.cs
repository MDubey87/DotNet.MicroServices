using Services.ShoppingCartApi.Models.Dto;

namespace Services.ShoppingCartApi.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
    }
}
