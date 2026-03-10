using Application.Web.Models;
using Application.Web.Services.Interfaces;
using LoginRequest = Application.Web.Models.LoginRequest;

namespace Application.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private readonly IConfiguration _config;
        private readonly string baseUrl;
        public AuthService(IBaseService baseService, IConfiguration config)
        {
            _baseService = baseService;
            _config = config;
            baseUrl = _config["ServiceUrls:AuthApi"] ?? string.Empty;
        }
        public async Task<ApiResponse> AssignRoleAsync(AssignRoleRequest request)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.POST,
                ApiUrl = $"{baseUrl}/api/auth/assign-role",
                Data = request
            });
        }

        public async Task<ApiResponse> LoginAsync(LoginRequest loginRequest)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.POST,
                ApiUrl = $"{baseUrl}/api/auth/login",
                Data = loginRequest
            });
        }

        public async Task<ApiResponse> RegisterAsync(RegistrationRequest registerRequest)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                RequestType = Utility.StaticDetails.RequestType.POST,
                ApiUrl = $"{baseUrl}/api/auth/register",
                Data = registerRequest
            });
        }
    }
}
