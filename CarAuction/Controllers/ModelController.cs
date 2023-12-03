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
            IEnumerable<Model> modelList = _db.Models;
            return View(modelList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Model model)
        {
            if (ModelState.IsValid)
            {
                _db.Models.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
