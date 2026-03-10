using Application.Web.Models;
using Application.Web.Services.Interfaces;
using Application.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        private readonly List<SelectListItem> RoleList = new() {
            new SelectListItem { Value = StaticDetails.RoleAdmin, Text = StaticDetails.RoleAdmin },
            new SelectListItem { Value = StaticDetails.RoleCustomer, Text = StaticDetails.RoleCustomer }
        };

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.LoginAsync(request);
                if (response != null && response.IsSuccess)
                {
                    LoginResponse loginResponse = new();

                   var dataString = response.Data.ToString();
                    if (!string.IsNullOrWhiteSpace(dataString))
                    {
                        loginResponse = JsonConvert.DeserializeObject<LoginResponse>(dataString) ?? new();
                        await SignInAsync(loginResponse);
                        _tokenProvider.SetToken(loginResponse.Token);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = response?.Message;
                }
            }
            return View(request);
        }
        public IActionResult Register()
        {            
            ViewBag.RoleList = RoleList;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RegisterAsync(request);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["Error"] = response?.Message;
                }
            }
            ViewBag.RoleList = RoleList;
            return View(request);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(LoginResponse loginResponse)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(loginResponse.Token);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,
                value: jwtToken.Claims.FirstOrDefault(u=>u.Type==JwtRegisteredClaimNames.Sub)?.Value),
                new Claim(JwtRegisteredClaimNames.Email,
                value: jwtToken.Claims.FirstOrDefault(u=>u.Type==JwtRegisteredClaimNames.Email)?.Value),
                new Claim(JwtRegisteredClaimNames.Name,
                value: jwtToken.Claims.FirstOrDefault(u=>u.Type==JwtRegisteredClaimNames.Name)?.Value),
                new Claim(ClaimTypes.Name,
                value: jwtToken.Claims.FirstOrDefault(u=>u.Type==JwtRegisteredClaimNames.Email)?.Value),
                new Claim(ClaimTypes.Role,
                value: jwtToken.Claims.FirstOrDefault(u=>u.Type=="role")?.Value)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
    }
}
