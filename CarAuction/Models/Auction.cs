using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarAuction.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

        public string AuctionDate { get; set; }
        [JsonIgnore]
        public List<Lot>? Lots { get; set; }

        public bool isActive { get; set; }
    }
}
