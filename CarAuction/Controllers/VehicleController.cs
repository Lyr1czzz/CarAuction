using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAuction.Controllers
{
    public class VehicleController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehicleController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var vehicleList = _db.Vehicles;

            foreach (var item in vehicleList)
            {
                item.Make = _db.Makes.FirstOrDefault(u => u.Id == item.MakeId);
            }

            return View(vehicleList);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> MakeDropDown = _db.Makes.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString(),
            //});
            ////ViewBag.MakeDropDown = MakeDropDown;
            //ViewData["MakeDropDown"] = MakeDropDown;
            //Vehicle vehicle = new Vehicle();

            VehicleVM vehicleVM = new VehicleVM()
            {
                Vehicle = new Vehicle(),
                MakeSelectList = _db.Makes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                })
            };

            if(id == null)
            {
                //this is for create
                return View(vehicleVM);
            }
            else
            {
                vehicleVM.Vehicle = _db.Vehicles.Find(id);
                if(vehicleVM == null)
                {
                    return NotFound();
                }
                return View(vehicleVM);
            }
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VehicleVM vehicleVM)
        {
            if (ModelState.IsValid)
            {
                // Получаем выбранный объект Make из базы данных
                Make selectedMake = _db.Makes.FirstOrDefault(m => m.Id == vehicleVM.Vehicle.MakeId);

                if (selectedMake != null)
                {
                    // Присваиваем выбранный объект Make в Vehicle
                    vehicleVM.Vehicle.Make = selectedMake;

                    _db.Vehicles.Add(vehicleVM.Vehicle);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Недопустимый идентификатор MakeId.");
                }
            }

            // Если модель не прошла проверку, возвращаем представление с сообщениями об ошибках
            return View(vehicleVM);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
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

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var make = _db.Makes.Find(id);

            if (make == null)
            {
                return NotFound();
            }

            _db.Makes.Remove(make);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }       
    }
}
