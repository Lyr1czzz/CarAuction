using CarAuction.Data.Base;
using CarAuction.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace CarAuction.Data.Services
{
    public class VehicleService : EntityBaseRepository<Vehicle>, IVehicleService
    {
        public VehicleService(AppDbContext context) : base(context) { }
    }
}
