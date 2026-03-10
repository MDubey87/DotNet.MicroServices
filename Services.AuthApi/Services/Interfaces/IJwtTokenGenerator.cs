using Services.AuthApi.Models;

namespace Services.AuthApi.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
