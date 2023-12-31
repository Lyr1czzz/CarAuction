using CarAuction.Data;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;  
        }

        public async Task<IActionResult> Index()
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
            DetailsVM detailsVM = new DetailsVM()
            {
                Vehicle = _db.Vehicles.Include(u=>u.Make).Include(u => u.Model).Where(u=>u.Id==id).FirstOrDefault(),
                ExistsInCard = false,
            };
            return View(detailsVM);
        }
    }
}
