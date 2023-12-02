using CarAuction.Data;
using CarAuction.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    public class MakeController : Controller
    {
        private readonly AppDbContext _db;

        public MakeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Make> objList = _db.Makes;
            return View(objList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }
    }
}
