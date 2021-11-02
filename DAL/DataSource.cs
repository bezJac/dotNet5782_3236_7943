using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    /// <summary>
    /// class manages data source level for DAL
    /// </summary>
    internal class DataSource
    {
        /// data bases 
        internal static List<BaseStation> Stations = new List<BaseStation>();
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> Charges = new List<DroneCharge>();

        static Random rnd = new Random();

        /// <summary>
        /// class manges parcel id
        /// </summary>
        internal class Config
        {    
            internal static int RunIdParcel = 0;
            internal static double DroneElecUseEmpty;
            internal static double DroneElecUseLight;
            internal static double DroneElecUseMedium;
            internal static double DroneElecUseHeavy;
            internal static double DroneHourlyChargeRate;

        }

        /// <summary>
        /// initialize the list 
        /// </summary>
        public static void Initialize()
        {
            /// array for stations names
            String[] locations = { "Jerusalem", "Haifa", "Tel Aviv", "Ashdod", "Eilat" };

            /// array for customer names
            string[] names = {"Wallace Winter","Cavan Cleveland","Muhammed Kirkpatrick","Deon Smyth","Vicky Mason","Mehak Tanner",
             "Gavin Powell","Ieuan Knapp","Charity Goodwin","Lara Huffman","Luther Mackenzie","Amalia Sykes",
             "Zachary Salas","Aleeza Holcomb","Patrycja Wooten","Sadiyah Nicholson","Martina Dodson","Gianluca Alford",
             "Rhys Whitehead","Anwen O'Brien","Marcie Marks","Lola Ruiz","Greta Wilkerson","Eryk Hassan","Sue Humphries",
             "Stanley Roche","Eesa Parsons","Elen Espinosa","Jia Aguilar","Kathryn Green", "Julia Henderson","Craig Carter",
             "Mary Robinson","Anne Patterson","Terry Price","Debra Lopez","Louise Taylor","Michelle Perez","Maria Williams",
              "Carl Bell","Betty Ward","Jessica Scott","Timothy Collins","Theresa Richardson" };

            /// array for drones models names
            string[] dronesModel = { "Yuneec H520", "DJI Mavic 2 Pro", "DJI Phantom 4", "Flyability Elios" };

            /// initialize 2-5 basestations and add to list
            int j = rnd.Next(2, 5);
            for (int i = 0; i < j; i++)
            {

                Stations.Add(new BaseStation
                {
                    Id = rnd.Next(1000, 10000),
                    Name = locations[i],
                    Lattitude = rnd.Next(29, 33) + rnd.NextDouble(),
                    Longitude = rnd.Next(33, 36) + rnd.NextDouble(),
                    NumOfSlots = rnd.Next(0, 6),
                });
            }

            /// initialize 5-10 drones and add to list
            j = rnd.Next(5, 10);
            for (int i = 0; i < j; i++)
            {
                Drones.Add(new Drone
                {
                    Id = rnd.Next(2000, 9999),
                    MaxWeight = (WeightCategories)rnd.Next(1, 4),
                    Model = dronesModel[rnd.Next(0, 4)],
                    //Status = (DroneStatus)2,
                   // Battery = rnd.Next(0, 101),
                })  ;
            }

            /// initialize 10-20 customers and add to list
            j = rnd.Next(10, 21);
            for (int i = 0; i < j; i++)
            {
                Customers.Add(new Customer
                {
                    Id = rnd.Next(100000, 1000000),
                    Name = names[rnd.Next(0, 43)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = rnd.Next(33, 36) + rnd.NextDouble(),
                    Lattitude = rnd.Next(29, 33) + rnd.NextDouble(),
                }) ;
            
            }

            /// initialize 10-15 parcels and add to list
            j = rnd.Next(10, 16);
            for (int i = 0; i < j; i++)
            {
                
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, Customers.Count())].Id,
                    TargetId = Customers[rnd.Next(0, Customers.Count())].Id,
                    Weight = (WeightCategories)rnd.Next(1, 4),
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    
                }) ;    
            }
        }
    }
}
 
