using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthApi.Models.Dto;
using Services.AuthApi.Services.Interfaces;

namespace Services.AuthApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDto _responseDto;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new ResponseDto();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            _responseDto.Message = result.Message;
            if (!result.IsSuccess)
            {
                _responseDto.IsSuccess = false;                
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result= await _authService.LoginAsync(request);
            if(result.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Invalid username or password";
                return Unauthorized(_responseDto);
            }
            _responseDto.Data = result;
            return Ok(_responseDto);
        }

        [HttpPost]
        [Route("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
        {
            var result = await _authService.AssignRoleAsync(request.Email,request.Role.ToUpper());
            _responseDto.Message = result.Message;
            if (!result.IsSuccess)
            {
                _responseDto.IsSuccess = false;
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

    }
}
