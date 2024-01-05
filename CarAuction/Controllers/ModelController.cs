using CarAuction.Data;
using CarAuction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
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

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var model = _db.Models.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Model model)
        {
            if (ModelState.IsValid)
            {
                _db.Models.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var model = _db.Models.Find(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var model = _db.Models.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            _db.Models.Remove(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
