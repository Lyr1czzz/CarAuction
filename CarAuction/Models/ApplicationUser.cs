using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class ApplicationUser
    {
        [Display(Name = "Full name")]
        public string FullName { get; set; }
    }
}
