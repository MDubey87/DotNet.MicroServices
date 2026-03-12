using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CouponApi.Models.Dto;
using Services.CouponApi.Repositories.Interfaces;

namespace Services.CouponApi.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    //[Authorize(Roles = "ADMIN")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ResponseDto _responseDto;
        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            var coupons = await _couponRepository.GetAll();
            _responseDto.Data = coupons;
            return Ok(_responseDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            var coupon = await _couponRepository.GetById(id);
            if(coupon == null)
            {
                _responseDto.Message = "Coupon not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Data = coupon;
            return Ok(_responseDto);
        }
        [HttpGet]
        [Route("{code}")]
        public async Task<ActionResult<ResponseDto>> GetByCode(string code)
        {
            var coupon = await _couponRepository.GetByCode(code);
            if (coupon == null)
            {
                _responseDto.Message = "Coupon not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Data = coupon;
            return Ok(_responseDto);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto>> Create([FromBody]RequestDto couponDto)
        {
            var coupon = await _couponRepository.Create(couponDto);            
            _responseDto.Data = coupon;
            _responseDto.Message = "Coupon created successfully";
            return Ok(_responseDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> Update([FromBody] RequestDto couponDto, int id)
        {
            var coupon = await _couponRepository.Update(couponDto, id);
            if (coupon == null)
            {
                _responseDto.Message = "Coupon not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Message = "Coupon updated successfully";
            return Ok(_responseDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
            var coupon = await _couponRepository.DeleteById(id);
            if (coupon == null)
            {
                _responseDto.Message = "Coupon not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Message = "Coupon deleted successfully";
            return Ok(_responseDto);
        }
    }
}
