using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using CarAuction.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CarAuction.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Vehicles = _db.Vehicles.Include(u => u.Make).Include(u => u.Model),
                Makes = _db.Makes
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                shoppingCarts = HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM()
            {
                Vehicle = _db.Vehicles.Include(u => u.Make).Include(u => u.Model).Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCard = false,
            };

            foreach (var item in shoppingCarts)
            {
                if (item.VehicleId == id)
                {
                    detailsVM.ExistsInCard = true;
                }
            }

            return View(detailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                shoppingCarts = HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCarts.Add(new ShoppingCart { VehicleId = id });
            HttpContext.Session.S(WC.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                shoppingCarts = HttpContext.Session.G<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemToRemove = shoppingCarts.SingleOrDefault(r => r.VehicleId == id);

            if (itemToRemove != null)
            {
                shoppingCarts.Remove(itemToRemove);
            }

            HttpContext.Session.S(WC.SessionCart, shoppingCarts);

            return RedirectToAction(nameof(Index));
        }
    }
}