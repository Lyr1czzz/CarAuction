using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;

        public CartController(AppDbContext db)
        {
            _db = db;
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

            return View(vehicles);
        }
    }
}
