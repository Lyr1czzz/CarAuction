using Microsoft.AspNetCore.Identity;

namespace CarAuction.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public List<Bid> Bids { get; set; }
    }
}
