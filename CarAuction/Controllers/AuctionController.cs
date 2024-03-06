using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class AuctionController : Controller
    {
        private readonly AppDbContext _db;

        public AuctionController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var auctionList = _db.Auctions.ToList();

            return View(auctionList);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
            AuctionVM auctionVM = new AuctionVM()
            {
                Auction = new Auction(),
                Vehicles = _db.Vehicles
                .Include(u => u.Make)
                .Include(u => u.Model)
                .Include(u => u.Images)
                .ToList()
            };


            if (id == null)
            {
                //this is for create
                
                return View(auctionVM);
            }
            else
            {
                auctionVM.Auction = _db.Auctions.Find(id);
                if (auctionVM == null)
                {
                    return NotFound();
                }
                return View(auctionVM);
            }
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert([FromForm] AuctionVM auctionVM)
        {
            if (ModelState.IsValid)
            {
                if (auctionVM.Auction.Id == 0)
                {
                    // Создание нового аукциона
                    var vehicleList = _db.Vehicles
                .Include(u => u.Make)
                .Include(u => u.Model)
                .Include(u => u.Images)
                .ToList();
                    foreach (var item in vehicleList)
                    {
                        item.AuctionId = auctionVM.Auction.Id;
                        item.Auction = auctionVM.Auction;
                    }
                    _db.Add(auctionVM.Auction);
                    _db.SaveChanges();
                }
                else
                {
                    // Обновление существующего аукциона
                    
                }
                return RedirectToAction("Index");
            }
            
            return View(auctionVM);
        }

        ////Get - Delete
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Vehicle vehicle = _db.Vehicles.Include(u=>u.Make).Include(u=>u.Model).Include(u => u.Images).FirstOrDefault(u=>u.Id==id);

        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(vehicle);
        //}

        ////Post - Delete
        //[HttpPost,ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePost(int? id)
        //{
        //    var obj = _db.Vehicles.Include(u => u.Images).FirstOrDefault(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehicleImages = obj.Images.ToList();
        //    if(vehicleImages.Count > 0)
        //    {
        //        foreach (var vehicleImage in vehicleImages)
        //        {
        //            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, vehicleImage.ImagePath);
        //            if (System.IO.File.Exists(fullPath))
        //            {
        //                System.IO.File.Delete(fullPath);
        //            }
        //        }
        //    }

        //    foreach (var item in vehicleImages)
        //    {
        //        _db.VehicleImages.Remove(item);
        //    }

        //    _db.Vehicles.Remove(obj);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //}       
    }
}
