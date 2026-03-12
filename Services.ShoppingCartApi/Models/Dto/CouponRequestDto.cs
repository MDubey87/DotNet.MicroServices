namespace Services.ShoppingCartApi.Models.Dto
{
    public class CouponRequestDto
    {
        public string userId { get; set; }=string.Empty;
        public string couponCode { get; set; } = string.Empty;
    }
}
