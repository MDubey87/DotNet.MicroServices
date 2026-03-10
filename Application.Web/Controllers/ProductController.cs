using Application.Web.Models;
using Application.Web.Services;
using Application.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Application.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
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
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateAsync(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = response?.Message;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int productId)
        {
            var response = await _productService.GetByIdAsync(productId);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                var dataString = response.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    ProductResponse? product = JsonConvert.DeserializeObject<ProductResponse>(dataString);
                    return View(product);
                }
            }
            else
            {
                TempData["Error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductResponse model)
        {
            var response = await _productService.DeleteByIdAsync(model.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = response?.Message;
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int productId)
        {
            var response = await _productService.GetByIdAsync(productId);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                var dataString = response.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    ProductResponse? coupon = JsonConvert.DeserializeObject<ProductResponse>(dataString);
                    return View(coupon);
                }
            }
            else
            {
                TempData["Error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductResponse model)
        {
            var response = await _productService.UpdateAsync(model);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = response?.Message;
            }
            return View(model);
        }
    }
}
