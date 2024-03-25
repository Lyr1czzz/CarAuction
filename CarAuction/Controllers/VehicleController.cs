using CarAuction.Data;
using CarAuction.Data.Enums;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
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
            var vehicleList = _db.Vehicles
                                 .Include(u => u.Make)
                                 .Include(u => u.Model)
                                 .Include(u => u.Images)
                                 .Include(u => u.Series)
                                 .Include(u => u.Engine)
                                 .ToList(); // Чтобы вызвать исключение здесь, если есть проблемы с запросом

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
                }),
                ModelSelectList = _db.Models.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                SeriesSelectList = _db.Series.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                EngineSelectList = _db.Engines.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                Vehicle_Type = new SelectList(Enum.GetValues(typeof(Vehicle_Type))),
                Vehicle_AirBags = new SelectList(Enum.GetValues(typeof(Vehicle_AirBags))),
                Vehicle_Body_Styles = new SelectList(Enum.GetValues(typeof(Vehicle_Body_Style))),
                Vehicle_Drive_Line_Types = new SelectList(Enum.GetValues(typeof(Vehicle_Drive_Line_Type))),
                Vehicle_Exteriors = new SelectList(Enum.GetValues(typeof(Vehicle_Exterior))),
                Vehicle_Fuel_Types = new SelectList(Enum.GetValues(typeof(Vehicle_Fuel_Type))),
                Vehicle_Interiors = new SelectList(Enum.GetValues(typeof(Vehicle_Interior))),
                Vehicle_Keys = new SelectList(Enum.GetValues(typeof(Vehicle_Key))),
                Vehicle_Loss = new SelectList(Enum.GetValues(typeof(Vehicle_Loss))),
                Vehicle_Primary_Damages = new SelectList(Enum.GetValues(typeof(Vehicle_Primary_Damage))),
                Vehicle_Secondary_Damages = new SelectList(Enum.GetValues(typeof(Vehicle_Secondary_Damage))),
                Vehicle_Start_Codes = new SelectList(Enum.GetValues(typeof(Vehicle_Start_Code))),
                Vehicle_Transmissions = new SelectList(Enum.GetValues(typeof(Vehicle_Transmission))),
        };
            if (id == null)
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
        public IActionResult Upsert([FromForm] VehicleVM vehicleVM, List<IFormFile>? uploadedFiles)
        {
            if (ModelState.IsValid)
            {
                if (vehicleVM.Vehicle.Id == 0)
                {
                    // Создание нового объявления автомобиля
                    _db.Add(vehicleVM.Vehicle);
                    _db.SaveChanges(); // Сохранить, чтобы получить ID для нового авто

                    if (uploadedFiles.Any())
                    {
                        foreach (var file in uploadedFiles)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);

                            using (var fileStream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            VehicleImage vehicleImage = new VehicleImage()
                            {
                                VehicleId = vehicleVM.Vehicle.Id,
                                ImagePath = Path.Combine("Files", fileName)
                            };
                            _db.VehicleImages.Add(vehicleImage);
                        }
                        _db.SaveChanges();
                    }
                }
                else
                {
                    // Обновление существующего объявления
                    var vehicleFromDb = _db.Vehicles.AsNoTracking().FirstOrDefault(u => u.Id == vehicleVM.Vehicle.Id);
                    if (vehicleFromDb == null)
                    {
                        return NotFound();
                    }

                    _db.Update(vehicleVM.Vehicle);

                    // Обработка загруженных файлов
                    if (uploadedFiles.Any())
                    {
                        // Удаляем все старые изображения
                        var imagesFromDb = _db.VehicleImages.Where(vi => vi.VehicleId == vehicleVM.Vehicle.Id);
                        _db.VehicleImages.RemoveRange(imagesFromDb);

                        foreach (var file in uploadedFiles)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, "Files", fileName);

                            using (var fileStream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            VehicleImage vehicleImage = new VehicleImage()
                            {
                                VehicleId = vehicleVM.Vehicle.Id,
                                ImagePath = Path.Combine("Files", fileName)
                            };
                            _db.VehicleImages.Add(vehicleImage);
                        }
                    }

                    _db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            // Загрузка значений для выпадающих списков в случае ошибки
            vehicleVM.MakeSelectList = _db.Makes.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            vehicleVM.ModelSelectList = _db.Models.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            vehicleVM.SeriesSelectList = _db.Series.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            vehicleVM.EngineSelectList = _db.Engines.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            vehicleVM.Vehicle_Type = new SelectList(Enum.GetValues(typeof(Vehicle_Type)));
            vehicleVM.Vehicle_AirBags = new SelectList(Enum.GetValues(typeof(Vehicle_AirBags)));
            vehicleVM.Vehicle_Body_Styles = new SelectList(Enum.GetValues(typeof(Vehicle_Body_Style)));
            vehicleVM.Vehicle_Drive_Line_Types = new SelectList(Enum.GetValues(typeof(Vehicle_Drive_Line_Type)));
            vehicleVM.Vehicle_Exteriors = new SelectList(Enum.GetValues(typeof(Vehicle_Exterior)));
            vehicleVM.Vehicle_Fuel_Types = new SelectList(Enum.GetValues(typeof(Vehicle_Fuel_Type)));
            vehicleVM.Vehicle_Interiors = new SelectList(Enum.GetValues(typeof(Vehicle_Interior)));
            vehicleVM.Vehicle_Keys = new SelectList(Enum.GetValues(typeof(Vehicle_Key)));
            vehicleVM.Vehicle_Loss = new SelectList(Enum.GetValues(typeof(Vehicle_Loss)));
            vehicleVM.Vehicle_Primary_Damages = new SelectList(Enum.GetValues(typeof(Vehicle_Primary_Damage)));
            vehicleVM.Vehicle_Secondary_Damages = new SelectList(Enum.GetValues(typeof(Vehicle_Secondary_Damage)));
            vehicleVM.Vehicle_Start_Codes = new SelectList(Enum.GetValues(typeof(Vehicle_Start_Code)));
            vehicleVM.Vehicle_Transmissions = new SelectList(Enum.GetValues(typeof(Vehicle_Transmission)));
            return View(vehicleVM);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Vehicle vehicle = _db.Vehicles
                                .Include(u => u.Make)
                                .Include(u => u.Model)
                                .Include(u => u.Series)
                                .Include(u => u.Engine)
                                .Include(u => u.Images)
                                .FirstOrDefault(u=>u.Id==id);
            
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        //Post - Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Vehicles.Include(u => u.Images).FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            var vehicleImages = obj.Images.ToList();
            if(vehicleImages.Count > 0)
            {
                foreach (var vehicleImage in vehicleImages)
                {
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, vehicleImage.ImagePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            foreach (var item in vehicleImages)
            {
                _db.VehicleImages.Remove(item);
            }

            _db.Vehicles.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }       
    }
}
