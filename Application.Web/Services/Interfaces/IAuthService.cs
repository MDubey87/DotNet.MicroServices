using Application.Web.Models;

namespace Application.Web.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<ApiResponse> LoginAsync(LoginRequest loginRequest);
        public Task<ApiResponse> RegisterAsync(RegistrationRequest registerRequest);
        public Task<ApiResponse> AssignRoleAsync(AssignRoleRequest registerRequest);
    }
}
