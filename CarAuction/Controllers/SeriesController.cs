using CarAuction.Data;
using CarAuction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class SeriesController : Controller
    {
        private readonly AppDbContext _db;

        public SeriesController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Series> seriesList = _db.Series;
            return View(seriesList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Series series)
        {
            if (ModelState.IsValid)
            {
                _db.Series.Add(series);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(series);
        }

        //Get - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var series = _db.Series.Find(id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Series series)
        {
            if (ModelState.IsValid)
            {
                _db.Series.Update(series);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(series);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var series = _db.Series.Find(id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var series = _db.Series.Find(id);

            if (series == null)
            {
                return NotFound();
            }

            _db.Series.Remove(series);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
