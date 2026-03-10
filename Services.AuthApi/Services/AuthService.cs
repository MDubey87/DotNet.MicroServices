using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.AuthApi.Data;
using Services.AuthApi.Models;
using Services.AuthApi.Models.Dto;
using Services.AuthApi.Services.Interfaces;

namespace Services.AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<(bool IsSuccess, string Message)> AssignRoleAsync(string userEmail, string roleName)
        {
            var user = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == userEmail);
            if (user == null)
            {
                return (false, "User not found");
            }
            var isRoleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!isRoleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return (result.Succeeded, result.Succeeded ? "Role assigned successfully"
                : string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _appDbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user == null)
            {
                return new LoginResponseDto { User = null, Token = string.Empty };
            }
            bool IsValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!IsValid)
            {
                return new LoginResponseDto { User = null, Token = string.Empty };
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new LoginResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email ?? string.Empty,
                    PhoneNumber = user.PhoneNumber ?? string.Empty,
                },
                Token = _jwtTokenGenerator.GenerateToken(user, roles)
            };
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(RegistrationRequestDto request)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                Name = request.Name,
                NormalizedEmail = request.Email.ToUpper(),
                PhoneNumber = request.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var isRoleExist = await _roleManager.RoleExistsAsync(request.Role);
                if (!isRoleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(request.Role));
                }
                await _userManager.AddToRoleAsync(user, request.Role);
            }
            return (result.Succeeded, result.Succeeded ?
                "User registered successfully" : string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}
