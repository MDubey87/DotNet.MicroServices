using Application.Web.Models;

namespace Application.Web.Services.Interfaces
{
    public interface ICouponService
    {
        public Task<ApiResponse> GetAllAsync();
        public Task<ApiResponse> GetByIdAsync(int couponId);
        public Task<ApiResponse> GetByCodeAsync(string couponCode);
        public Task<ApiResponse> CreateAsync(CouponRequest coupon);
        public Task<ApiResponse> UpdateAsync(CouponRequest coupon, int couponId);
        public Task<ApiResponse> DeleteByIdAsync(int couponId);
    }
}
