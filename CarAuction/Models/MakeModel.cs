using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuction.Models
{
    public class MakeModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Make Type")]
        public int MakeId { get; set; }

        [ForeignKey("MakeId")]
        public virtual Make? Make { get; set; }

        [Display(Name = "Make Type")]
        public int ModelId { get; set; }

        [ForeignKey("ModelId")]
        public virtual Model? Model { get; set; }
    }
}
