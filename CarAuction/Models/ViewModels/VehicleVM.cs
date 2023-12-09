using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAuction.Models.ViewModels
{
    public class VehicleVM
    {
        public Vehicle Vehicle { get; set; }
        public IEnumerable<SelectListItem>? MakeSelectList { get; set; }
    }
}
