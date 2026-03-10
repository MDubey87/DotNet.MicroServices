using System.Text.Json.Serialization;

namespace Services.AuthApi.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class AssignRoleRequestDto
    {
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
