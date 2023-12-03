using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class Make
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Display Order for category must be greate than 0")]
        public int DisplayOrder { get; set; }
    }
}
