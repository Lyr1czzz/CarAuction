using CarAuction.Data;
using CarAuction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class EngineController : Controller
    {
        private readonly AppDbContext _db;

        public EngineController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Engine> enginesList = _db.Engines;
            return View(enginesList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Engine engine)
        {
            if (ModelState.IsValid)
            {
                _db.Engines.Add(engine);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(engine);
        }

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var engine = _db.Engines.Find(id);
            if (engine == null)
            {
                return NotFound();
            }

            return View(engine);
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Engine engine)
        {
            if (ModelState.IsValid)
            {
                _db.Engines.Update(engine);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(engine);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var engine = _db.Engines.Find(id);
            if (engine == null)
            {
                return NotFound();
            }

            return View(engine);
        }

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var engine = _db.Engines.Find(id);

            if (engine == null)
            {
                return NotFound();
            }

            _db.Engines.Remove(engine);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
