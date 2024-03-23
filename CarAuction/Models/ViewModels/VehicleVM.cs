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

        public IEnumerable<Vehicle_AirBags>? AirBags { get; set; }
        public IEnumerable<Vehicle_Body_Style>? BodyStyles { get; set; }
        public IEnumerable<Vehicle_Drive_Line_Type>? DriveLineTypes { get; set; }
        public IEnumerable<Vehicle_Exterior>? Exteriors { get; set; }
        public IEnumerable<Vehicle_Fuel_Type>? FuelTypes { get; set; }
        public IEnumerable<Vehicle_Interior>? Interiors { get; set; }
        public IEnumerable<Vehicle_Key>? Keys { get; set; }
        public IEnumerable<Vehicle_Loss>? Loss { get; set; }
        public IEnumerable<Vehicle_Primary_Damage>? PrimaryDamages { get; set; }
        public IEnumerable<Vehicle_Secondary_Damage>? SecondaryDamages { get; set; }
        public IEnumerable<Vehicle_Start_Code>? StartCodes { get; set; }
        public IEnumerable<Vehicle_Transmission>? Transmissions { get; set; }
    }
}