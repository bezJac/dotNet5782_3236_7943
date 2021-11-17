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
            internal static int RunIdParcel = 20000;
            internal static double DroneElecUseEmpty = 2.5;
            internal static double DroneElecUseLight = 3;
            internal static double DroneElecUseMedium = 3.5;
            internal static double DroneElecUseHeavy = 4;
            internal static double DroneHourlyChargeRate =67;
            

        }

        /// <summary>
        /// initialize the list 
        /// </summary>
        public static void Initialize()
        {
            ///// array for stations names
            //String[] locations = { "Jerusalem", "Haifa", "Tel Aviv", "Ashdod", "Eilat" };
            double[] randCoordinates = { 0.00134, 0.00876, 0.001432, 0.002345, 0.001987, 0.00976, 0.001532, 0.006432, 0.002845, 0.00201 };
            double[] randCoordinates2 = { 0.003456, 0.004567, 0.005932, 0.001234, 0.003217, 0.004786, 0.004321, 0.002331, 0.003921, 0.00299 };
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
          
                Stations.Add(new BaseStation
                {
                    Id = rnd.Next(1000, 10000),
                    Name = "Ramat Aviv Gimmel",
                    Lattitude = 32.12534,
                    Longitude = 34.79722,
                    NumOfSlots = 20,
                });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Jaffa Port",
                Lattitude = 32.05234,
                Longitude = 34.74900,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Ha-Medina Square",
                Lattitude = 32.08672,
                Longitude = 34.78960,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Kiryat Shalom",
                Lattitude = 32.04115,
                Longitude = 34.77621,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Tel Aviv University",
                Lattitude = 32.10640,
                Longitude = 34.80088,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Neve Tzedek",
                Lattitude = 32.06239,
                Longitude = 34.76428,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "City Center",
                Lattitude = 32.08187,
                Longitude = 34.77226,
                NumOfSlots = 20,
            });


            /// initialize 5-10 drones and add to list
            int j = 20;
            for (int i = 0; i < j; i++)
            {
                Drones.Add(new Drone
                {
                    Id = rnd.Next(2000, 9999),
                    MaxWeight = (WeightCategories)rnd.Next(1, 4),
                    Model = dronesModel[rnd.Next(0, 4)],
                })  ;
            }

            /// initialize 10-20 customers and add to list
             j = 10;
            for (int i = 0; i < j; i++)
            {
                Customers.Add(new Customer
                {
                    Id = rnd.Next(100000, 1000000),
                    Name = names[rnd.Next(0, 43)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = 34.74861 + randCoordinates2[rnd.Next(10)],
                    Lattitude = 32.03669 + randCoordinates[rnd.Next(10)],

                }) ;
            
            }
            for (int i = 0; i < j; i++)
            {
                Customers.Add(new Customer
                {
                    Id = rnd.Next(10000000, 100000000),
                    Name = names[rnd.Next(0, 43)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = 34.77900 + randCoordinates2[rnd.Next(10)],
                    Lattitude = 32.07582 + randCoordinates[rnd.Next(10)] ,
                });

            }

            /// initialize 10-15 parcels and add to list
            j = 10;
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
                    DroneId = 0,
                    
                }) ;
                
                
            }
            j = 5;
            for (int k = 0; k < j; k++)
            {
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, Customers.Count())].Id,
                    TargetId = Customers[rnd.Next(0, Customers.Count())].Id,
                    DroneId = Drones[k].Id,
                    Weight = (WeightCategories)Drones[k].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now.AddMinutes(rnd.Next(60, 180)),

                });
            }
            for (int k = 0; k < 5; k++)
            {
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, Customers.Count())].Id,
                    TargetId = Customers[rnd.Next(0, Customers.Count())].Id,
                    DroneId = Drones[k + 5].Id,
                    Weight = (WeightCategories)Drones[k + 5].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now.AddMinutes(rnd.Next(60,180)),
                    PickedUp = DateTime.Now.AddMinutes(rnd.Next(180, 500)),
                });
            }
            for (int k = 0; k < 5; k++)
            {
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, Customers.Count())].Id,
                    TargetId = Customers[rnd.Next(0, Customers.Count())].Id,
                    DroneId = Drones[k + 10].Id,
                    Weight = (WeightCategories)Drones[k + 10].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now.AddMinutes(rnd.Next(60, 180)),
                    PickedUp = DateTime.Now.AddMinutes(rnd.Next(180, 500)),
                    Delivered = DateTime.Now.AddMinutes(rnd.Next(500,680)),
                });
            }
        }
    }
}
 
