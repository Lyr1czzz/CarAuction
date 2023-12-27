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
            var vehicleList = _db.Vehicles.ToList();

            foreach (var item in vehicleList)
            {
                item.Make = _db.Makes.FirstOrDefault(u => u.Id == item.MakeId);
            }

            return View(vehicleList);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
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
        public IActionResult Upsert([FromForm] VehicleVM vehicleVM, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if(vehicleVM.Vehicle.Id == 0) 
                {

                    if (uploadedFile != null)
                    {
                        // Creating
                        string path = "/Files/" + uploadedFile.FileName;
                        
                        using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            uploadedFile.CopyTo(fileStream);
                        }

                        Make selectedMake = _db.Makes.FirstOrDefault(m => m.Id == vehicleVM.Vehicle.MakeId);

                        if (selectedMake != null)
                        {
                            vehicleVM.Vehicle.Make = selectedMake;
                            vehicleVM.Vehicle.Path = "Files/" + uploadedFile.FileName;
                            _db.Vehicles.Add(vehicleVM.Vehicle);
                            _db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Недопустимый идентификатор MakeId.");
                        }
                    }
                }
                else
                {
                    // Updating
                    var objFromDb = _db.Vehicles.FirstOrDefault(u => u.Id == vehicleVM.Vehicle.Id);
                    if (uploadedFile is not null)
                    {
                        string path = "/Files/" + uploadedFile.FileName;

                        var oldFile = Path.Combine(path, objFromDb.Path);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(_webHostEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            uploadedFile.CopyTo(fileStream);
                        }
                        vehicleVM.Vehicle.Path = "Files/" + uploadedFile.FileName;

                    }
                    _db.SaveChanges();
                    return RedirectToAction("Index");
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
