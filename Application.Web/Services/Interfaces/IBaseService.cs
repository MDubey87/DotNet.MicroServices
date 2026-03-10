using Application.Web.Models;

namespace Application.Web.Services.Interfaces
{
    public interface IBaseService
    {
        public Task<ApiResponse> SendAsync(ApiRequest apiRequest);
    }
}
