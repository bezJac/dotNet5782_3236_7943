using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public class DataSource
    {
        
        internal static BaseStation[] baseStations = new BaseStation[5];
        internal static Drone[] drones = new Drone[10];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];

        internal class Config
        {
            internal static int baseIndex = 0;
            internal static int droneIndex = 0;
            internal static int customerIndex = 0;
            internal static int parcelIndex = 0;
            internal static int runIdParcel = 0;
        }


        // needs fixing 
        public static void Initialize()
        {
            Random rnd = new Random();
            int j = rnd.Next(2, 5);
            for (int i = 0; i < j; i++)
            {
                baseStations[i].Id = i+100;
                baseStations[i].Name = (i + 65).ToString();
                baseStations[i].Lattitude = rnd.Next(29, 33) + rnd.NextDouble();
                baseStations[i].Longitude = rnd.Next(33, 36) + rnd.NextDouble();
                baseStations[i].NumOfSlots = rnd.Next(0, 10);
                Config.baseIndex++;

            }               
            j = rnd.Next(5, 10);
            for (int i = 0; i < j; i++)
            {
                drones[i].Id = 1000+i;
                drones[i].MaxWeight = (WeightCategories)rnd.Next(0, 2);
                drones[i].Model = rnd.Next(5233, 5242);
                drones[i].Status = (DroneStatus)rnd.Next(0, 2);
                drones[i].Battery = rnd.Next(0, 100);
                Config.droneIndex++;
            }
            j = rnd.Next(10, 100);
            for (int i = 0; i < j; i++)
            {

                customers[i].Id = rnd.Next(10000,99999);
                customers[i].Name= rnd.Next(65, 90).ToString()+ rnd.Next(65, 90).ToString();
                customers[i].Phone=$"0{ rnd.Next(50,58)}{ rnd.Next(1000000, 9999999)}" ;
                customers[i].Longitude = rnd.Next(33, 36) + rnd.NextDouble();
                customers[i].Latitude = rnd.Next(29, 33) + rnd.NextDouble();
                Config.customerIndex++;
            }
            j = rnd.Next(10, 100);
            for (int i = 0; i < j; i++)
            {
                parcels[i].Id = rnd.Next(100000, 999999);
                parcels[i].SenderId= rnd.Next(10000, 99999);
                parcels[i].TargetId= rnd.Next(10000, 99999);
                parcels[i].Weight = (WeightCategories)rnd.Next(0, 2);
                parcels[i].Priority=(Priorities)rnd.Next(0, 2);
                parcels[i].DroneId= rnd.Next(1000, 9999);
               
                parcels[i].Requested = new DateTime(2022, rnd.Next(1, 12), rnd.Next(1, 31), rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
                parcels[i].Scheduled = parcels[i].Requested.AddHours(rnd.Next(6, 12));
                parcels[i].PickedUp = parcels[i].Scheduled.AddHours(rnd.Next(12, 24));
                parcels[i].Delivered = parcels[i].PickedUp.AddHours(rnd.Next(1, 4));
                Config.parcelIndex++;
            }
            Config.runIdParcel = parcels.Length+1;


        }
    }
}
 
