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
            IEnumerable<Make> makeList = _db.Makes;
            return View(makeList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Make make)
        {
            if (ModelState.IsValid)
            {
                _db.Makes.Add(make);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(make);
        }

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var make = _db.Makes.Find(id);
            if (make == null)
            {
                return NotFound();
            }

            return View(make);
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Make make)
        {
            if (ModelState.IsValid)
            {
                _db.Makes.Update(make);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(make);
        }
    }
}
