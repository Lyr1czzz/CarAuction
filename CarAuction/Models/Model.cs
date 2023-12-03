using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CarAuction.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
