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
            IEnumerable<Vehicle> vehicleList = _db.Vehicles;

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

        //Post - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(VehicleVM vehicleVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(vehicleVM.Vehicle.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath; 
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    vehicleVM.Vehicle.Image = fileName + extension;

                    _db.Vehicles.Add(vehicleVM.Vehicle);
                }
                else
                {
                    //Updating
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
