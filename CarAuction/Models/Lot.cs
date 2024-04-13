using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class Lot

    {
        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public virtual List<Bid> Bids { get; set; }
        public Vehicle? Vehicle { get; set; }

        public int AuctionId { get; set; }
        public Auction? Auction { get; set; }

        public double FinalCost { get; set; }

        public bool isSaled { get; set; }
    }
}
