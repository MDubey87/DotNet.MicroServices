using Services.CouponApi.Models.Dto;

namespace Services.CouponApi.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        public Task<IEnumerable<CouponDto>> GetAll();
        public Task<CouponDto?> GetById(int id);
        public Task<CouponDto?> GetByCode(string code);
        public Task<CouponDto> Create(RequestDto coupon);
        public Task<bool?> Update(RequestDto coupon,int couponId);
        public Task<bool?> DeleteById(int id);
    }
}
