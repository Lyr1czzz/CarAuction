using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using CarAuction.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarAuction.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public VehicleUserVM VehicleUserVM { get; set; }

        public CartController(AppDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null 
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> vehicleInCart = shoppingCarts.Select(i=>i.VehicleId).ToList();

            List<Vehicle> vehicles = _db.Vehicles.Where(u => vehicleInCart.Contains(u.Id)).ToList();
            _emailSender.SendEmailAsync("billyroll03@gmail.com", "qerw", "dsfgfsdgsgs");
            return View(vehicles);
        }

        public IActionResult Remove(int id)
        {
            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(u=>u.VehicleId == id));

            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> vehicleInCart = shoppingCarts.Select(i => i.VehicleId).ToList();
            List<Vehicle> vehicles = _db.Vehicles.Where(u => vehicleInCart.Contains(u.Id)).ToList();

            VehicleUserVM = new VehicleUserVM()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value),
                Vehicles = vehicles
            };

            return View(VehicleUserVM);
        }
    }
}
