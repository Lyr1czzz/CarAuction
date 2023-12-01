﻿using System.ComponentModel.DataAnnotations;

namespace CarAuction.Models
{
    public class Make
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
