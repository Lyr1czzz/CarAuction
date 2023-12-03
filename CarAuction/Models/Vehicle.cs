using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuction.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set;}

        public string Image { get; set;}

        [Display(Name= "Make Type")]
        public int MakeId { get; set; }

        [ForeignKey("MakeId")]
        public virtual Make Make { get; set; }
    }
}
