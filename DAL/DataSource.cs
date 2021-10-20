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
        internal static List<DroneCharge> Charges = new List<DroneCharge>();

        static Random rnd = new Random();

        internal class Config
        {    
            internal static int RunIdParcel = 0;
        }


        // needs fixing 
        public static void Initialize()
        {
            String[] locations = { "Jerusalem", "Haifa", "Tel Aviv", "Ashdod", "Eilat" };
            string[] names = {"Wallace Winter","Cavan Cleveland","Muhammed Kirkpatrick","Deon Smyth","Vicky Mason","Mehak Tanner",
                              "Gavin Powell","Ieuan Knapp","Charity Goodwin","Lara Huffman","Luther Mackenzie","Amalia Sykes",
                              "Zachary Salas","Aleeza Holcomb","Patrycja Wooten","Sadiyah Nicholson","Martina Dodson","Gianluca Alford",
                              "Rhys Whitehead","Anwen O'Brien","Marcie Marks","Lola Ruiz","Greta Wilkerson","Eryk Hassan","Sue Humphries",
                              "Stanley Roche","Eesa Parsons","Elen Espinosa","Jia Aguilar","Kathryn Green", "Julia Henderson","Craig Carter",
                              "Mary Robinson","Anne Patterson","Terry Price","Debra Lopez","Louise Taylor","Michelle Perez","Maria Williams",
                              "Carl Bell","Betty Ward","Jessica Scott","Timothy Collins","Theresa Richardson" };
            int j = rnd.Next(2, 5);
            for (int i = 0; i < j; i++)
            {

                Stations.Add(new BaseStation
                {
                    Id = rnd.Next(1000, 10000),
                    Name = locations[i],
                    Lattitude = rnd.Next(29, 33) + rnd.NextDouble(),
                    Longitude = rnd.Next(33, 36) + rnd.NextDouble(),
                    NumOfSlots = rnd.Next(0, 10),
                });
            }

            j = rnd.Next(5, 10);
            for (int i = 0; i < j; i++)
            {
                Drones.Add(new Drone
                {
                    Id = rnd.Next(2000, 9999),
                    MaxWeight = (WeightCategories)rnd.Next(0, 3),
                    //Model ="",
                    Status = (DroneStatus)rnd.Next(0, 3),
                    Battery = rnd.Next(0, 101),
                });
            }
        
                
            
            j = rnd.Next(10, 100);
            for (int i = 0; i < j; i++)
            {
                Customers.Add(new Customer
                {
                    Id = ++Config.RunIdParcel,
                    Name = names[rnd.Next(0, 46)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = rnd.Next(33, 36) + rnd.NextDouble(),
                    Lattitude = rnd.Next(29, 33) + rnd.NextDouble(),
                }) ;
            
            }
                
            
            j = rnd.Next(10, 100);
            for (int i = 0; i < j; i++)
            {
                Parcels.Add(new Parcel
                {
                    Id = rnd.Next(100000, 999999),
                    SenderId = rnd.Next(10000, 99999),
                    TargetId = rnd.Next(10000, 99999),
                    Weight = (WeightCategories)rnd.Next(0, 3),
                    Priority = (Priorities)rnd.Next(0, 3),
                    DroneId = rnd.Next(1000, 9999),
                });

                  
                
            }
            


        }

    }
}
 
