using CarAuction.Data;
using CarAuction.Data.Enums;
using CarAuction.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CarAuction.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                
                //Movies
                if (!context.Vehicles.Any())
                {
                    context.Vehicles.AddRange(new List<Vehicle>()
                    {
                        new Vehicle()
                        {
                            Name = "Car1",
                            Description = "This is the Life description",
                            Price = 39.50,
                            ImageURL = "https://www.topgear.com/sites/default/files/2022/07/13.jpg",
                            //Make = Make.FORD,
                            //Model = Model.MUSTANG,
                            Vehicle_Condition_Type = Vehicle_Condition_Type.Run_and_Drive,
                            Vehicle_Type = Vehicle_Type.Automobiles
                        },
                        new Vehicle()
                        {
                            Name = "Car2",
                            Description = "This is the Life description",
                            Price = 139.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-3.jpeg",
                            //Make = Make.FORD,
                            //Model = Model.MUSTANG,
                            Vehicle_Condition_Type = Vehicle_Condition_Type.Starts,
                            Vehicle_Type = Vehicle_Type.Automobiles
                        },
                        new Vehicle()
                        {
                           Name = "Car3",
                            Description = "This is the Life description",
                            Price = 359.50,
                            ImageURL = "https://www.topgear.com/sites/default/files/2022/07/13.jpg",
                            //Make = Make.BMW,
                            //Model = Model.M3,
                            Vehicle_Condition_Type = Vehicle_Condition_Type.Run_and_Drive,
                            Vehicle_Type = Vehicle_Type.Automobiles
                        },
                        new Vehicle()
                        {
                            Name = "Car4",
                            Description = "This is the Life description",
                            Price = 394.50,
                            ImageURL = "http://dotnethow.net/images/movies/movie-3.jpeg",
                            //Make = Make.FERRARI,
                            //Model = Model.CALIFORNIA,
                            Vehicle_Condition_Type = Vehicle_Condition_Type.Starts,
                            Vehicle_Type = Vehicle_Type.Automobiles
                        },
                    });
                    context.SaveChanges();
                }
               
            }

        }

    }
}