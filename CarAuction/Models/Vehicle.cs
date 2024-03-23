using CarAuction.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuction.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        public string VIN { get; set; }

        public int Odometer { get; set; }

        public int Year { get; set; }

        public Vehicle_AirBags AirBags { get; set; }
        public Vehicle_Body_Style BodyStyle { get; set; }
        public Vehicle_Drive_Line_Type DriveLineType { get; set; }
        public Vehicle_Exterior Exterior { get; set; }
        public Vehicle_Fuel_Type FuelType { get; set; }
        public Vehicle_Interior Interior { get; set; }
        public Vehicle_Key Key { get; set; }
        public Vehicle_Loss Loss { get; set; }
        public Vehicle_Primary_Damage PrimaryDamage { get; set; }
        public Vehicle_Secondary_Damage SecondaryDamage { get; set; }
        public Vehicle_Start_Code StartCode { get; set; }
        public Vehicle_Transmission Transmission { get; set; }

        [Range(1, int.MaxValue)]
        public double Price { get; set;}

        [Display(Name = "Engine Type")]
        public int EngineId { get; set; }

        [ForeignKey("EngineId")]
        public virtual Engine? Engine { get; set; }

        [Display(Name = "Series Type")]
        public int SeriesId { get; set; }

        [ForeignKey("SeriesId")]
        public virtual Series? Series { get; set; }

        [Display(Name= "Make Type")]
        public int MakeId { get; set; }

        [ForeignKey("MakeId")]
        public virtual Make? Make { get; set; }

        [Display(Name = "Model Type")]
        public int ModelId { get; set; }

        [ForeignKey("ModelId")]
        public virtual Model? Model { get; set; }

        public virtual ICollection<VehicleImage>? Images { get; set; }
    }
}
