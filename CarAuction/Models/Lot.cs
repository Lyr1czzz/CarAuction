using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarAuction.Models
{
    public class Lot

    {
        [Key]
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public virtual List<Bid> Bids { get; set; }
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }

        public int AuctionId { get; set; }
        [JsonIgnore]
        public Auction? Auction { get; set; }

        public double FinalCost { get; set; }

        public bool isSaled { get; set; }
    }
}
