﻿namespace CarAuction.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }
        public IEnumerable<Make> Makes { get; set; }
    }
}
