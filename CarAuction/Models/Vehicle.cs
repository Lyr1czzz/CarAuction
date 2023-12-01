using CarAuction.Data.Base;
using CarAuction.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuction.Models
{
    public class Vehicle : IEntityBase
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ImageURL { get; set; }

        public Vehicle_Condition_Type Vehicle_Condition_Type { get; set; }

        public Vehicle_Type Vehicle_Type { get; set; }
    }
}
