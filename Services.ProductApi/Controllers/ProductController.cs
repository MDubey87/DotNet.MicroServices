using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ProductApi.Models.Dto;
using Services.ProductApi.Repositories.Interfaces;

namespace Services.ProductApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ResponseDto _responseDto;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> Get()
        {
            var coupons = await _productRepository.GetAll();
            _responseDto.Data = coupons;
            return Ok(_responseDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ResponseDto>> GetById(int id)
        {
            var coupon = await _productRepository.GetById(id);
            if (coupon == null)
            {
                _responseDto.Message = "Product not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Data = coupon;
            return Ok(_responseDto);
        }        

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> Create([FromBody] RequestDto request)
        {
            var coupon = await _productRepository.Create(request);
            _responseDto.Data = coupon;
            _responseDto.Message = "Product created successfully";
            return Ok(_responseDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> Update([FromBody] RequestDto request, int id)
        {
            var coupon = await _productRepository.Update(request, id);
            if (coupon == null)
            {
                _responseDto.Message = "Product not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Message = "Product updated successfully";
            return Ok(_responseDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseDto>> Delete(int id)
        {
            var coupon = await _productRepository.DeleteById(id);
            if (coupon == null)
            {
                _responseDto.Message = "Product not found";
                _responseDto.IsSuccess = false;
                return NotFound(_responseDto);
            }
            _responseDto.Message = "Product deleted successfully";
            return Ok(_responseDto);
        }
    }
}
