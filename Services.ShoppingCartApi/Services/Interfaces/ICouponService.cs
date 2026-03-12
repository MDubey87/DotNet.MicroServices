using Services.ShoppingCartApi.Models.Dto;

namespace Services.ShoppingCartApi.Services.Interfaces
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
