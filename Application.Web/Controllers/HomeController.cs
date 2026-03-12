using Application.Web.Models;
using Application.Web.Services;
using Application.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Application.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            var products = new List<ProductResponse>();
            var response = await _productService.GetAllAsync();
            if (response != null && response.IsSuccess && response.Data != null)
            {
                var dataString = response.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    products = JsonConvert.DeserializeObject<List<ProductResponse>>(dataString) ?? new List<ProductResponse>();
                }
            }
            else
            {
                TempData["Error"] = response?.Message;
            }
            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetail(int productId)
        {
            ProductResponse? model = new();

            ApiResponse? response = await _productService.GetByIdAsync(productId);

            if (response != null && response.IsSuccess && response.Data != null)
            {
                var dataString = response.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    model = JsonConvert.DeserializeObject<ProductResponse>(dataString) ?? new ProductResponse();
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
