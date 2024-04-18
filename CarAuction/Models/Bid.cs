using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarAuction.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        public double Amount { get; set; }

        public string? AuctionDate { get; set; } = string.Empty!;

        [JsonIgnore]
        public virtual Lot? Lot { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
    }
}
