using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarAuction.Models
{
    public class Engine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
