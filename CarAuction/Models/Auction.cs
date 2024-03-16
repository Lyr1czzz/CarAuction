using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

        public string AuctionDate { get; set; }

        public virtual List<Lot>? Lots { get; set; }

        public bool isActive { get; set; }
    }
}
