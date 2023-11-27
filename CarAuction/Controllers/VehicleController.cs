using CarAuction.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _service;

        public VehicleController(IVehicleService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }
    }
}
