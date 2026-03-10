using System.Text.Json.Serialization;

namespace Application.Web.Models
{
    public class RegistrationRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class AssignRoleRequest
    {
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
