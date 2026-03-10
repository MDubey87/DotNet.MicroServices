using Application.Web.Models;
using Application.Web.Services.Interfaces;
using Application.Web.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Application.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ApiResponse> SendAsync(ApiRequest apiRequest)
        {
            try
            {

                HttpClient client = _httpClientFactory.CreateClient("APIClient");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenProvider.GetToken());
                message.RequestUri = new Uri(apiRequest.ApiUrl);
                message.Method = apiRequest.RequestType switch
                {
                    StaticDetails.RequestType.GET => HttpMethod.Get,
                    StaticDetails.RequestType.POST => HttpMethod.Post,
                    StaticDetails.RequestType.PUT => HttpMethod.Put,
                    StaticDetails.RequestType.DELETE => HttpMethod.Delete,
                    _ => throw new NotImplementedException()
                };
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                }

                var response = await client.SendAsync(message);
                var apiResponseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiResponseContent);
                if (apiResponse is null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Message = GetResponseMessage(response.StatusCode),
                        StatusCode = response.StatusCode
                    };
                }
                apiResponse.StatusCode = response.StatusCode;
                return apiResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"{GetResponseMessage(HttpStatusCode.InternalServerError)}: {ex.Message}",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        private string GetResponseMessage(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return "Request successful.";
                case HttpStatusCode.Created:
                    return "Resource created successfully.";
                case HttpStatusCode.BadRequest:
                    return "Bad request. Please check your input.";
                case HttpStatusCode.Unauthorized:
                    return "Unauthorized";
                case HttpStatusCode.Forbidden:
                    return "Forbidden. You do not have permission to access this resource.";
                case HttpStatusCode.NotFound:
                    return "Resource not found.";
                case HttpStatusCode.InternalServerError:
                    return "Internal server error. Please try again later.";
                default:
                    return $"Unexpected status code: {statusCode}";
            }
        }
    }
}
