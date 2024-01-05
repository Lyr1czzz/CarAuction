namespace CarAuction.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public int MileageFilter { get; set; }
    }
}
