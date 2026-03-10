using Services.CouponApi.Models;
using Services.CouponApi.Models.Dto;

namespace Services.CouponApi.Mapper
{
    public static class Mapper
    {
        public static CouponDto ToCouponDto(this Coupon coupon)
        {
            return new CouponDto
            {
                CouponId = coupon.CouponId,
                CouponCode = coupon.CouponCode,
                DiscountAmount = coupon.DiscountAmount,
                MinAmount = coupon.MinAmount
            };
        }
        public static Coupon ToCouponEntity(this CouponDto coupon)
        {
            return new Coupon
            {
                CouponId = coupon.CouponId,
                CouponCode = coupon.CouponCode,
                DiscountAmount = coupon.DiscountAmount,
                MinAmount = coupon.MinAmount
            };
        }
        public static Coupon ToCouponEntity(this RequestDto coupon)
        {
            return new Coupon
            {
                CouponCode = coupon.CouponCode,
                DiscountAmount = coupon.DiscountAmount,
                MinAmount = coupon.MinAmount
            };
        }
    }
}
