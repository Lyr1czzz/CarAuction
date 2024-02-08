using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        public int AuctionId { get; set; }

        public string UserId { get; set; }

        public double Amount { get; set; }

        public virtual Auction Auction { get; set; }
    }
}
