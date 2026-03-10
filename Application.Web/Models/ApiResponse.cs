using System.Net;

namespace Application.Web.Models
{
    public class ApiResponse
    {
        public object? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public HttpStatusCode StatusCode { get; set; }
    }
}
