using Services.ProductApi.Models;
using Services.ProductApi.Models.Dto;

namespace Services.ProductApi.Mapper
{
    public static class Mapper
    {
        public static ProductDto ToProductDto(this Product coupon)
        {
           return new ProductDto
            {
                ProductId = coupon.ProductId,
                Name = coupon.Name,
                Price = coupon.Price,
                Description = coupon.Description,
                CategoryName = coupon.CategoryName,
                ImageUrl = coupon.ImageUrl,
                ImageLocalPath = coupon.ImageLocalPath
            };
        }
        public static Product ToCouponEntity(this ProductDto coupon)
        {
            return new Product
            {
                ProductId = coupon.ProductId,
                Name = coupon.Name,
                Price = coupon.Price,
                Description = coupon.Description,
                CategoryName = coupon.CategoryName,
                ImageUrl = coupon.ImageUrl,
                ImageLocalPath = coupon.ImageLocalPath
            };
        }
        public static Product ToCouponEntity(this RequestDto product)
        {
            return new Product
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = product.CategoryName,
                ImageUrl = product.ImageUrl,
                ImageLocalPath = product.ImageLocalPath
            };
        }
    }
}
