using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using IDAL.DO;

namespace DalObject
{
   internal class DataSource
    {

        internal static List<BaseStation> Stations = new List<BaseStation>();
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();

        static Random rnd = new Random();

        internal class Config
        {
            
            internal static int RunIdParcel = 0;
        }


        // needs fixing 
        public static void Initialize()
        {
            int j = rnd.Next(2, 5);
            for (int i = 0; i < j; i++)
            {
                Stations.Add(new BaseStation
                {
                    Id = rnd.Next(1000, 10000),
                    Name = (i + 65).ToString(),
                    Lattitude = rnd.Next(29, 33) + rnd.NextDouble(),
                    Longitude = rnd.Next(33, 36) + rnd.NextDouble(),
                    NumOfSlots = rnd.Next(0, 10),
                });
             }

            j = rnd.Next(5, 10);
            for (int i = 0; i < j; i++)
            {
                Drones[i].Id = rnd.Next(2000, 9999);
                Drones[i].MaxWeight = (WeightCategories)rnd.Next(0, 3);
                Drones[i].Model = rnd.Next(5233, 5256);
                Drones[i].Status = (DroneStatus)rnd.Next(0, 3);
                Drones[i].Battery = rnd.Next(0, 101);
                Config.DroneIndex++;
            }
            j = rnd.Next(10, 100);
            for (int i = 0; i < j; i++)
            {

                Customers[i].Id = rnd.Next(100000000, 100000000);
                Customers[i].Name= rnd.Next(65, 90).ToString()+ rnd.Next(65, 90).ToString();
                Customers[i].Phone= $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}";
                Customers[i].Longitude = rnd.Next(33, 36) + rnd.NextDouble();
                Customers[i].Latitude = rnd.Next(29, 33) + rnd.NextDouble();
                Config.CustomerIndex++;
            }
            j = rnd.Next(10, 100);
            for (int i = 0; i < j; i++)
            {
                Parcels[i].Id = rnd.Next(100000, 999999);
                Parcels[i].SenderId= rnd.Next(10000, 99999);
                Parcels[i].TargetId= rnd.Next(10000, 99999);
                Parcels[i].Weight = (WeightCategories)rnd.Next(0, 3);
                Parcels[i].Priority=(Priorities)rnd.Next(0, 3);
                Parcels[i].DroneId= rnd.Next(1000, 9999);
               
                Parcels[i].Requested = new DateTime(2022, rnd.Next(1, 12), rnd.Next(1, 31), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
                Parcels[i].Scheduled = Parcels[i].Requested.AddHours(rnd.Next(6, 12));
                Parcels[i].PickedUp = Parcels[i].Scheduled.AddHours(rnd.Next(12, 24));
                Parcels[i].Delivered = Parcels[i].PickedUp.AddHours(rnd.Next(1, 4));
                Config.ParcelIndex++;
            }
            Config.RunIdParcel = Parcels.Length+1;


        }

    }
}
 
