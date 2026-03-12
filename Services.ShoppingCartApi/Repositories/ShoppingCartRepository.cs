using Microsoft.EntityFrameworkCore;
using Services.ShoppingCartApi.Data;
using Services.ShoppingCartApi.Mapper;
using Services.ShoppingCartApi.Models;
using Services.ShoppingCartApi.Models.Dto;
using Services.ShoppingCartApi.Repositories.Interfaces;
using Services.ShoppingCartApi.Services.Interfaces;
using System.Reflection.PortableExecutable;

namespace Services.ProductApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public readonly AppDbContext _context;
        public readonly IProductService _productService;
        public readonly ICouponService _couponService;
        public ShoppingCartRepository(AppDbContext context, IProductService productService, ICouponService couponService)
        {
            _context = context;
            _productService = productService;
            _couponService = couponService;
        }

        public async Task<bool> ApplyCoupon(CouponRequestDto request)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(h => h.UserId == request.userId);
            if (cartHeader == null) { return false; }
            cartHeader.CouponCode = request.couponCode;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartDto> CartUpsert(RequestDto request)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1) Get existing header or create it
                var cartHeader = await _context.CartHeaders
                    .FirstOrDefaultAsync(h => h.UserId == request.CartHeader.UserId);

                if (cartHeader == null)
                {
                    cartHeader = request.CartHeader.ToCartHeaderEntity();
                    _context.CartHeaders.Add(cartHeader);
                }
                else
                {
                    cartHeader.CouponCode = request.CartHeader.CouponCode;
                }
                await _context.SaveChangesAsync();

                // 2) If no details, we’re done
                if (request.CartDetails != null && request.CartDetails.Any())
                {
                    // 3) Load existing details once (avoid per-item DB roundtrips)
                    var productIds = request.CartDetails
                        .Select(d => d.ProductId)
                        .Distinct()
                        .ToList();

                    var existingDetails = await _context.CartDetails
                        .Where(d => d.CartHeaderId == cartHeader.CartHeaderId && productIds.Contains(d.ProductId))
                        .ToListAsync();

                    // 4) Upsert each incoming detail
                    foreach (var incoming in request.CartDetails)
                    {
                        var dbDetail = existingDetails.FirstOrDefault(d => d.ProductId == incoming.ProductId);

                        if (dbDetail == null)
                        {
                            // Insert new detail
                            incoming.CartHeaderId = cartHeader.CartHeaderId;
                            _context.CartDetails.Add(incoming.ToCartDetailEntity());
                        }
                        else
                        {
                            // Update existing detail (persist to DB!)
                            dbDetail.Count = incoming.Count;

                            // If you track other mutable fields, set them here too
                            // dbDetail.UpdatedAt = DateTime.UtcNow; etc.
                        }
                    }

                    // 5) Save once
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return new CartDto
                {
                    CartHeader = cartHeader.ToCartHeaderDto(),
                    CartDetails = await _context.CartDetails
                        .Where(d => d.CartHeaderId == cartHeader.CartHeaderId)
                        .Select(d => d.ToCartDetailDto())
                        .ToListAsync()
                };
            }
            catch
            {
                // Rollback then rethrow to preserve stack trace
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<CartDto?> GetCart(string userId)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(h => h.UserId == userId);
            if (cartHeader == null)
                return null;
            var cartDetails = await _context.CartDetails.Where(d => d.CartHeaderId == cartHeader.CartHeaderId).ToListAsync();
            var cartDto = new CartDto
            {
                CartHeader = cartHeader.ToCartHeaderDto(),
                CartDetails = cartDetails.Select(d => d.ToCartDetailDto())
            };
            var products = await _productService.GetProducts();
            var coupon = new CouponDto();
            if (!string.IsNullOrWhiteSpace(cartHeader.CouponCode))
                coupon = await _couponService.GetCouponByCode(cartHeader.CouponCode);
            foreach (var item in cartDto.CartDetails)
            {
                item.Product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                cartDto.CartHeader.CartTotal += item.Count * (item.Product == null ? 0.0 : item.Product.Price);
            }
            if (cartDto.CartHeader.CartTotal >= coupon.MinAmount)
            {
                cartDto.CartHeader.Discount = coupon.DiscountAmount;
                cartDto.CartHeader.CartTotal -= coupon.DiscountAmount;
            }
            return cartDto;

        }

        public async Task<bool> RemoveCart(int cartDetailId)
        {
            var existingCartDetail = await _context.CartDetails
                       .FirstAsync(d => d.CartDetailId == cartDetailId);


            if (existingCartDetail == null)
                return false;

            var cartHeaderIds = await _context.CartDetails
                       .Where(d => d.CartHeaderId == existingCartDetail.CartHeaderId)
                       .Select(d => d.CartHeaderId)
                       .ToListAsync();

            _context.CartDetails.Remove(existingCartDetail);

            if (cartHeaderIds.Count() == 1)
            {
                var cartHeaders = await _context.CartHeaders.Where(c => cartHeaderIds.Contains(c.CartHeaderId)).ToListAsync();
                _context.CartHeaders.RemoveRange(cartHeaders);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCoupon(CouponRequestDto request)
        {
            var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(h => h.UserId == request.userId);
            if (cartHeader == null) { return false; }
            cartHeader.CouponCode = string.Empty;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
