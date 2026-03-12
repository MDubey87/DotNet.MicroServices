using System.Text.Json.Serialization;

namespace Services.ShoppingCartApi.Models.Dto
{
    public class RequestDto
    {
        public CartHeaderRequestDto CartHeader { get; set; }
        public IEnumerable<CartDetailRequestDto>? CartDetails { get; set; }
    }

    public class CartHeaderRequestDto
    {
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
    }
    public class CartDetailRequestDto
    {
        [JsonIgnore]
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }

}
