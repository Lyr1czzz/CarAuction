namespace CarAuction.Models.ViewModels
{
    public class VehicleUserVM
    {
        public VehicleUserVM()
        {
            Vehicles = new List<Vehicle>();
        }

        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
