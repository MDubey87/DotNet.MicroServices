using Microsoft.AspNetCore.Identity;

namespace Services.AuthApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
