using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

        public string AuctionDate { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public virtual List<Bid> Bids { get; set; }
    }
}
