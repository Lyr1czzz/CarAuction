using CarAuction.Data;
using CarAuction.Models;
using CarAuction.Models.ViewModels;
using CarAuction.Utility;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Claims;

namespace CarAuction.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public VehicleUserVM VehicleUserVM { get; set; }

        public CartController(AppDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> vehicleInCart = shoppingCarts.Select(i => i.VehicleId).ToList();

            List<Vehicle> vehicles = _db.Vehicles.Where(u => vehicleInCart.Contains(u.Id))
                .Include(u => u.Images)
                .Include(u => u.Make)
                .Include(u => u.Model)
                .Include(u => u.Series)
                .Include(u => u.Engine)
                .ToList();
            return View(vehicles);
        }

        public IActionResult Remove(int id)
        {
            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCarts.Remove(shoppingCarts.FirstOrDefault(u => u.VehicleId == id));

            HttpContext.Session.Set(WC.SessionCart, shoppingCarts);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> vehicleInCart = shoppingCarts.Select(i => i.VehicleId).ToList();
            List<Vehicle> vehicles = _db.Vehicles.Where(u => vehicleInCart.Contains(u.Id))
                .Include(u => u.Images)
                .Include(u => u.Make)
                .Include(u => u.Model)
                .Include(u => u.Series)
                .Include(u => u.Engine).ToList();

            VehicleUserVM = new VehicleUserVM()
            {
                ApplicationUser = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value),
                Vehicles = vehicles
            };

            return View(VehicleUserVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shoppingCarts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart).Count > 0)
            {
                //session exsits
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == claim.Value);

            SendRegistrationCodeByEmail(user.Email, GenerateRegistrationCode());
            return Redirect(nameof(Index));
        }

        private async Task SendRegistrationCodeByEmail(string email, string registrationCode)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("CarAuction", "carauction228@mail.ru"));
                message.To.Add(new MailboxAddress("лох бобовый", email));
                message.Subject = "Регистрация";
                message.Body = new TextPart("plain")
                {
                    Text = $"Код регистрации: {registrationCode}"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.mail.ru", 465, SecureSocketOptions.Auto);
                    client.Authenticate("carauction228@mail.ru", "RH4Jne0vRrvWCSgdh7uf");
                    await client.SendAsync(message);
                    client.Disconnect(true);
                }


                //MessageBox.Show("Код регистрации был отправлен на вашу почту. Пожалуйста, проверьте свою почту и введите код для завершения регистрации.");
            }
            catch (Exception ex)
            {

                //MessageBox.Show($"Ошибка при отправке письма: {ex.Message}");
            }
        }

        private string GenerateRegistrationCode()
        {
            // Генерация случайного числа или кода, например, с помощью класса Random или Guid
            string registrationCode = Guid.NewGuid().ToString().Substring(0, 6);
            return registrationCode;
        }
    }
}
