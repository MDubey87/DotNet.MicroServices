namespace Services.ShoppingCartApi.Models.Dto
{
    public class CouponDto
    {
        public int CouponId { get; set; }        
        public string CouponCode { get; set; } = string.Empty;
        public double DiscountAmount { get; set; } = 0.0;
        public int MinAmount { get; set; }
    }
}
