using Services.AuthApi.Models.Dto;

namespace Services.AuthApi.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        public Task<(bool IsSuccess,string Message)> RegisterAsync(RegistrationRequestDto request);
        public Task<(bool IsSuccess, string Message)> AssignRoleAsync(string userEmail, string roleName);
    }
}
