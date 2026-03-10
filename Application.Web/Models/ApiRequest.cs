using static Application.Web.Utility.StaticDetails;

namespace Application.Web.Models
{
    public class ApiRequest
    {
        public RequestType RequestType { get; set; } = RequestType.GET;
        public string ApiUrl { get; set; } = string.Empty;
        public object? Data { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
