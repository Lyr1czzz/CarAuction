namespace CarAuction.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            Vehicle = new Vehicle();
        }
        public Vehicle Vehicle { get; set; }
        public bool ExistsInCard {  get; set; }
    }
}
