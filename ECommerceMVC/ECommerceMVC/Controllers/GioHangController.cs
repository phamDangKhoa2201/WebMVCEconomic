using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Helpers;

namespace ECommerceMVC.Controllers
{
    public class GioHangController : Controller
    {
        private readonly Hshop2023Context db;

        public GioHangController(Hshop2023Context context) {
            db=context;
        }
        public List<CartItemVM> Cart => HttpContext.Session.Get<List<CartItemVM>>
            (MySetting.CART_KEY) ?? new List<CartItemVM>();
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int id, int quantity =1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.Id == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p =>p.MaHh == id);
                if(hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItemVM
                {
                    Id = hangHoa.MaHh,
                    TenHh = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");   
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.Id == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");   
        }
    }
}
