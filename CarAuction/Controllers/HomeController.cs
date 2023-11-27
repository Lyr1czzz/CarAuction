using Microsoft.AspNetCore.Mvc;

namespace CarAuction.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
