using CarAuction.Data.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAuction.Models.ViewModels
{
    public class VehicleVM
    {
        public Vehicle Vehicle { get; set; }
        public IEnumerable<SelectListItem>? MakeSelectList { get; set; }
        public IEnumerable<SelectListItem>? ModelSelectList { get; set; }
        public IEnumerable<SelectListItem>? SeriesSelectList { get; set; }
        public IEnumerable<SelectListItem>? EngineSelectList { get; set; }
        public List<IFormFile>? UploadedFiles { get; set; }
        public IEnumerable<VehicleImage>? VehicleImages { get; set; }

        public IEnumerable<SelectListItem>? Vehicle_Type { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_AirBags { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Body_Styles { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Drive_Line_Types { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Exteriors { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Fuel_Types { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Interiors { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Keys { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Loss { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Primary_Damages { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Secondary_Damages { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Start_Codes { get; set; }
        public IEnumerable<SelectListItem>? Vehicle_Transmissions { get; set; }
    }
}