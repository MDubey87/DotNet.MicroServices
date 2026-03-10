using Microsoft.EntityFrameworkCore;
using Services.CouponApi.Data;
using Services.CouponApi.Mapper;
using Services.CouponApi.Models.Dto;
using Services.CouponApi.Repositories.Interfaces;

namespace Services.CouponApi.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        public readonly AppDbContext _context;
        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }
        // Project directly to DTOs to avoid loading entities unnecessarily
        public async Task<IEnumerable<CouponDto>> GetAll()
        {
            return await _context.Coupons
                .AsNoTracking()
                .Select(c => c.ToCouponDto())
                .ToListAsync();
        }

        public async Task<CouponDto?> GetById(int id)
        {
            return await _context.Coupons
                .AsNoTracking()
                .Where(c => c.CouponId == id)
                .Select(c => c.ToCouponDto())
                .FirstOrDefaultAsync();
        }

        public async Task<CouponDto?> GetByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var normalized = code.Trim().ToLowerInvariant();

            return await _context.Coupons
                .AsNoTracking()
                .Where(c => string.Equals(c.CouponCode.ToLower(), normalized))
                .Select(c => c.ToCouponDto())
                .FirstOrDefaultAsync();
        }

        public async Task<CouponDto> Create(RequestDto coupon)
        {
            var newCoupon = coupon.ToCouponEntity();
            _context.Coupons.Add(newCoupon);
            return await _context.SaveChangesAsync()
                .ContinueWith(t => newCoupon.ToCouponDto());
        }

        public async Task<bool?> Update(RequestDto coupon, int couponId)
        {
            var existing = _context.Coupons.Find(couponId);
            if (existing == null)
                return null;
            existing.CouponCode = coupon.CouponCode;
            existing.DiscountAmount = coupon.DiscountAmount;
            existing.MinAmount = coupon.MinAmount;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> DeleteById(int id)
        {
            var existing = _context.Coupons.Find(id);
            if (existing == null)
                return null;
            _context.Coupons.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
