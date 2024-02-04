using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAuction.Models.ViewModels
{
    public class VehicleVM
    {
        public Vehicle Vehicle { get; set; }
        public IEnumerable<SelectListItem>? MakeSelectList { get; set; }
        public IEnumerable<SelectListItem>? ModelSelectList { get; set; }
        public List<IFormFile>? UploadedFiles { get; set; }

        public IEnumerable<VehicleImage>? VehicleImages { get; set; }
    }
}