using Services.ShoppingCartApi.Models.Dto;

namespace Services.ShoppingCartApi.Repositories.Interfaces
{
    public interface IShoppingCartRepository
    {
        public Task<CartDto> CartUpsert(RequestDto request);
        public Task<bool> RemoveCart(int cartDetailId);
        public Task<CartDto?> GetCart(string userId);

        public Task<bool> ApplyCoupon(CouponRequestDto request);
        public Task<bool> RemoveCoupon(CouponRequestDto request);

    }
}
