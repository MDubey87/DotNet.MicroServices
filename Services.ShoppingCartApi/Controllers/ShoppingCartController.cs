using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ShoppingCartApi.Models.Dto;
using Services.ShoppingCartApi.Repositories.Interfaces;

namespace Services.ShoppingCartApi.Controllers
{
    [Route("api/shopping-cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _repository;
        private readonly ResponseDto _responseDto;
        public ShoppingCartController(IShoppingCartRepository repository)
        {
            _repository = repository;
            _responseDto = new ResponseDto();
        }

        [HttpPost]
        public async Task<IActionResult> CartUpsert([FromBody] RequestDto request)
        {
            try
            {
                var result = await _repository.CartUpsert(request);
                _responseDto.Data = result;
                _responseDto.Message = "Cart upserted successfully.";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error upserting cart: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
            }
            
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveCart([FromBody] int cartDetailId)
        {
            try
            {
                var result = await _repository.RemoveCart(cartDetailId);
                if (!result)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Cart detail not found or could not be removed.";
                    return NotFound(_responseDto);
                }
                _responseDto.Data = result;
                _responseDto.Message = "Cart removed successfully.";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error removing cart: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
            }

        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                var result = await _repository.GetCart(userId);
                _responseDto.Data = result;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error getting cart: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
            }

        }

        [HttpPost("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CouponRequestDto request)
        {
            try
            {
                var result = await _repository.ApplyCoupon(request);
                if (!result)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Cart detail not found.";
                    return NotFound(_responseDto);
                }
                _responseDto.Data = result;
                _responseDto.Message = "Coupon Applied successfully.";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error applying coupon: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
            }
        }
        [HttpPost("remove-coupon")]
        public async Task<IActionResult> RemoveCoupon([FromBody] CouponRequestDto request)
        {
            try
            {
                var result = await _repository.RemoveCoupon(request);
                if (!result)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Cart detail not found.";
                    return NotFound(_responseDto);
                }
                _responseDto.Data = result;
                _responseDto.Message = "Coupon Removed successfully.";
                return Ok(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Error removing coupon: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
            }
        }
    }
}
