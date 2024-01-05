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

        public int Mileage { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set;}

        public string Path { get; set; } = string.Empty;

        [Display(Name= "Make Type")]
        public int MakeId { get; set; }

        [ForeignKey("MakeId")]
        public virtual Make? Make { get; set; }

        [Display(Name = "Model Type")]
        public int ModelId { get; set; }

        [ForeignKey("ModelId")]
        public virtual Model? Model { get; set; }
    }
}
