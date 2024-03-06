using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuction.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        public double Amount { get; set; }

        public string? AuctionDate { get; set; } = string.Empty!;

        public virtual Lot? Lot { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
