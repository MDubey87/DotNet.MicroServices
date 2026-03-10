namespace Application.Web.Models
{
    public class CouponRequest
    {
        public string CouponCode { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
