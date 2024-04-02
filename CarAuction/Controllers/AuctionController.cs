using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public IActionResult AuctionList()
        {
            var auctionList = _db.Auctions.Where(u=>u.isActive == true).ToList();

            return View(auctionList);
        }

        public Lot GetCurrentLot()
        {
            return _db.Lots
                .Include(l => l.Bids)
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Make) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Model) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Series) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Engine) // Связь один к одному или один ко многим
                .Include(l => l.Vehicle)
                    .ThenInclude(v => v.Images) // Связь один ко многим
                .Include(l => l.Auction)
                .FirstOrDefault(l => l.Auction.isActive && !l.isSaled);
        }

        //Get - Details
        public IActionResult Details(int? id)
        {
            AuctionVM auctionVM = new AuctionVM()
            {
                Auction = new Auction(),
                Vehicles = _db.Vehicles
                .Include(u => u.Make)
                .Include(u => u.Model)
                .Include(u => u.Series)
                .Include(u => u.Engine)
                .Include(u => u.Images)
                .ToList()
            };


            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var currentLot = GetCurrentLot();
                auctionVM.Auction = _db.Auctions.Find(id);
                auctionVM.CurrentLot = currentLot;
                auctionVM.Auction.Lots = _db.Lots.Where(u => u.AuctionId == id).ToList();
                if (auctionVM == null)
                {
                    return NotFound();
                }
                return View(auctionVM);
            }
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
                .Include(u => u.Series)
                .Include(u => u.Engine)
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
                auctionVM.Auction.Lots = _db.Lots.Where(u => u.AuctionId == id).ToList();
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
        public IActionResult Upsert(AuctionVM model, string selectedVehicles, string selectedForDeleteVehicles, bool AuctionIsOn)
        {
            // Десериализовываем ID выбранных машин
            var selectVehicleIds = JsonConvert.DeserializeObject<List<int>>(selectedVehicles);
            var deleteVehicleIds = JsonConvert.DeserializeObject<List<int>>(selectedForDeleteVehicles);
            var auction = model.Auction;
            auction.isActive = AuctionIsOn;
            var auctionVM = model;
            auctionVM.Vehicles = _db.Vehicles
                .Include(u => u.Make)
                .Include(u => u.Model)
                .Include(u => u.Series)
                .Include(u => u.Engine)
                .Include(u => u.Images)
                .ToList();

            var currentLot = GetCurrentLot();
            auctionVM.CurrentLot = currentLot;
            if (ModelState.IsValid)
            {
                // Если ID аукциона == 0, значит это новый аукцион
                if (auction.Id == 0)
                {
                    // Создаём новый лот аукциона
                    _db.Add(auction);
                }
                else
                {
                    // Обновляем существующий аукцион
                    _db.Update(auction);

                    
                }

                // Сохраняем изменения, чтобы получить ID для новых аукционов
                _db.SaveChanges();

                // Добавляем выбранные машины к аукциону
                foreach (var vehicleId in selectVehicleIds)
                {
                    _db.Lots.Add(new Lot
                    {
                        VehicleId = vehicleId,
                        AuctionId = auction.Id // используем ID только что созданного или обновленного аукциона
                    });
                }

                foreach (var vehicleId in deleteVehicleIds)
                {
                    foreach (var lot in _db.Lots.Where(u=>u.Id == vehicleId))
                    {
                        if(lot.Bids != null)
                        foreach (var bid in lot.Bids)
                        {
                            _db.Bids.Remove(bid);
                        }
                        _db.Lots.Remove(lot);
                    }
                }


                // Подтверждаем изменения в базе данных
                _db.SaveChanges();

                // Перенаправляем пользователя на страницу с информацией о аукционе или на другой маршрут
                return RedirectToAction("Index"); // Замените "Index" на нужное название отображаемой страницы после создания аукциона
            }

            // Если форма не валидна, покажем её снова с текущими значениями
            return View(auctionVM);
        }

        //Get - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Auction auction = _db.Auctions.Include(u => u.Lots).FirstOrDefault(u => u.Id == id);

            if (auction == null)
            {
                return NotFound();
            }
            return View(auction);
        }

        //Post - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Auctions.Include(u => u.Lots).FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            foreach (var item in obj.Lots)
            {
                foreach (var bid in item.Bids)
                {
                    _db.Bids.Remove(bid);
                }
                _db.Lots.Remove(item);
            }

            _db.Auctions.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
