using Newtonsoft.Json;
using Services.ShoppingCartApi.Models.Dto;
using Services.ShoppingCartApi.Services.Interfaces;

namespace Services.ShoppingCartApi.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("CouponApi");
            var response = await client.GetAsync($"api/coupon/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (responseDto != null && responseDto.IsSuccess && responseDto.Data != null)
            {
                var dataString = responseDto.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    var products = JsonConvert.DeserializeObject<CouponDto>(dataString);
                    return products ?? new CouponDto();
                }
            }
            return new CouponDto();
        }
    }
}
