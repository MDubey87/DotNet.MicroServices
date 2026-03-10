using Application.Web.Models;
using Application.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Application.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> Index()
        {
            var coupons = new List<CouponResponse>();
            var response = await _couponService.GetAllAsync();
            if (response != null && response.IsSuccess && response.Data != null)
            {
                var dataString = response.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    coupons = JsonConvert.DeserializeObject<List<CouponResponse>>(dataString) ?? new List<CouponResponse>();
                }
            }
            else
            {
                TempData["Error"] = response?.Message;
            }
            return View(coupons);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CouponRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.CreateAsync(model);
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
        public async Task<IActionResult> Delete(int couponId)
        {
            var response = await _couponService.GetByIdAsync(couponId);
            if (response != null && response.IsSuccess && response.Data != null)
            {
                var dataString = response.Data.ToString();
                if (!string.IsNullOrWhiteSpace(dataString))
                {
                    CouponResponse? coupon = JsonConvert.DeserializeObject<CouponResponse>(dataString);                    
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
        public async Task<IActionResult> Delete(CouponResponse model)
        {
            var response = await _couponService.DeleteByIdAsync(model.CouponId);
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
