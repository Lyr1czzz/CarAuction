using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CarAuction.Models
{
    public class VehicleImage
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }

        public string? ImagePath { get; set; }
        
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
