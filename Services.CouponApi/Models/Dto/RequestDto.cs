namespace Services.CouponApi.Models.Dto
{
    public class RequestDto
    {
        public string CouponCode { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
