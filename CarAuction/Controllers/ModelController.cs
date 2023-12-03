using CarAuction.Data;
using CarAuction.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    public class ModelController : Controller
    {
        private readonly AppDbContext _db;

        public ModelController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Model> objList = _db.Models;
            return View(objList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Model obj)
        {
            _db.Models.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
