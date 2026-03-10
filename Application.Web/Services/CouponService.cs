using Application.Web.Models;
using Application.Web.Services.Interfaces;

namespace Application.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        private readonly IConfiguration _config;
        private readonly string baseUrl;
        public CouponService(IBaseService baseService, IConfiguration config)
        {
            _baseService = baseService;
            _config = config;
            baseUrl = _config["ServiceUrls:CouponApi"] ?? string.Empty;
        }
        public async Task<ApiResponse> CreateAsync(CouponRequest coupon)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.POST,
                ApiUrl = $"{baseUrl}/api/coupon",
                Data = coupon
            });
        }

        public async Task<ApiResponse> DeleteByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.DELETE,
                ApiUrl = $"{baseUrl}/api/coupon/{couponId}"
            });
        }

        public async Task<ApiResponse> GetAllAsync()
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.GET,
                ApiUrl = $"{baseUrl}/api/coupon"
            });
        }

        public async Task<ApiResponse> GetByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.GET,
                ApiUrl = $"{baseUrl}/api/coupon/{couponCode}"
            });
        }

        public async Task<ApiResponse> GetByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.GET,
                ApiUrl = $"{baseUrl}/api/coupon/{couponId}"
            });
        }

        public async Task<ApiResponse> UpdateAsync(CouponRequest coupon, int couponId)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.PUT,
                ApiUrl = $"{baseUrl}/api/coupon/{couponId}",
                Data = coupon
            });
        }
    }
}
