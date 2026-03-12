using Services.ShoppingCartApi.Models;
using Services.ShoppingCartApi.Models.Dto;

namespace Services.ShoppingCartApi.Mapper
{
    public static class Mapper
    {
        public static CartHeaderDto ToCartHeaderDto(this CartHeader cartHeader)
        {
            return new CartHeaderDto
            {
                CartHeaderId = cartHeader.CartHeaderId,
                UserId = cartHeader.UserId,
                CouponCode = cartHeader.CouponCode,
                CartTotal = cartHeader.CartTotal,
                Discount = cartHeader.Discount
            };
        }
        public static CartHeader ToCartHeaderEntity(this CartHeaderDto cartHeader)
        {
            return new CartHeader
            {
                CartHeaderId = cartHeader.CartHeaderId,
                UserId = cartHeader.UserId,
                CouponCode = cartHeader.CouponCode,
                CartTotal = cartHeader.CartTotal,
                Discount = cartHeader.Discount
            };
        }

        public static CartDetailDto ToCartDetailDto(this CartDetail cartDetail)
        {
            return new CartDetailDto
            {
                CartDetailId = cartDetail.CartDetailId,
                CartHeaderId = cartDetail.CartHeaderId,
                ProductId = cartDetail.ProductId,
                Product = cartDetail.Product,
                Count = cartDetail.Count,
                CartHeader = cartDetail.CartHeader != null ?
                cartDetail.CartHeader.ToCartHeaderDto() : null
            };
        }
        public static CartDetail ToCartDetailEntity(this CartDetailDto cartDetail)
        {
            return new CartDetail
            {
                CartDetailId = cartDetail.CartDetailId,
                CartHeaderId = cartDetail.CartHeaderId,
                ProductId = cartDetail.ProductId,
                Product = cartDetail.Product,
                Count = cartDetail.Count,
                CartHeader = cartDetail.CartHeader != null ?
                cartDetail.CartHeader.ToCartHeaderEntity() : new CartHeader()
            };
        }

        public static CartHeader ToCartHeaderEntity(this CartHeaderRequestDto cartDetail)
        {
            return new CartHeader
            {
                UserId = cartDetail.UserId,
                CouponCode = cartDetail.CouponCode,
                CartTotal = cartDetail.CartTotal,
                Discount = cartDetail.Discount
            };
        }
        public static CartDetail ToCartDetailEntity(this CartDetailRequestDto cartDetail)
        {
            return new CartDetail
            {
                CartHeaderId = cartDetail.CartHeaderId,
                ProductId = cartDetail.ProductId,
                Count = cartDetail.Count
            };
        }
    }
}
