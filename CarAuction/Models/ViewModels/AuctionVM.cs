using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarAuction.Models.ViewModels
{
    public class AuctionVM
    {
        public Auction Auction { get; set; }

        public IEnumerable<Vehicle>? Vehicles { get; set; }
    }
}