using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAuction.Models.ViewModels
{
    public class AuctionVM
    {
        public Auction Auction { get; set; }

        public List<Vehicle>? Vehicles { get; set; }
    }
}