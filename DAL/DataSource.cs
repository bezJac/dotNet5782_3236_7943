using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;


namespace DalObject
{
    /// <summary>
    /// class contains data lists in DAL
    /// </summary>
    internal class DataSource
    {
        
        internal static List<BaseStation> Stations = new List<BaseStation>();
        internal static List<Drone> Drones = new List<Drone>();
        internal static List<Customer> Customers = new List<Customer>();
        internal static List<Parcel> Parcels = new List<Parcel>();
        internal static List<DroneCharge> Charges = new List<DroneCharge>();

        static Random rnd = new Random();

        /// <summary>
        /// class contains  parcel id and configurations dor drone electric use
        /// </summary>
        internal class Config
        {
            internal static int RunIdParcel = 20000;
            internal static double DroneElecUseEmpty = 2.5;
            internal static double DroneElecUseLight = 3;
            internal static double DroneElecUseMedium = 3.5;
            internal static double DroneElecUseHeavy = 4;
            internal static double DroneHourlyChargeRate = 67;


        }

        /// <summary>
        ///  function manualy initializes the list 
        /// </summary>
        public static void Initialize()
        {
            /// <summary>
            ///  random mesurments to add to station and customer location coordinates
            /// </summary>
            double[] randCoordinates = { 0.00134, 0.00876, 0.001432, 0.002345, 0.001987, 0.00976, 0.001532, 0.006432, 0.002845, 0.00201 };
            double[] randCoordinates2 = { 0.003456, 0.004567, 0.005932, 0.001234, 0.003217, 0.004786, 0.004321, 0.002331, 0.003921, 0.00299 };

            string[] firstNames = {"Wallace","Cavan","Elina","Deon","Vicky","Mehak",
             "Gavin","Ieuan","Charity","Lara","Luther","Amalia","Svetlana",
             "Zachary","Aleeza","Patrycja","Sadiyah","Martina","Gianluca",
             "Rhys","Anwen","Marcie","Lola","Greta","Eryk","Sue","Sarah",
             "Stanley","Eesa","Elen","Jia","Kathryn", "Julia","Carter","Oliver",
             "Mary","Anne","Terry","Debra","Louise","Michelle","Maria","Gordon",
              "Carl","Betty","Jessica","Timothy","Theresa","Katie","Simona","David","Donny" };

            string[] lastNames = {"Velasquez","Farley","Keller","Moreno","Hayes","Matthew","Rich","Haney","Alvarez","Faulkner","Wilkinson","Joseph",
             "Little","Reid","Salas","Herrera","Hamilton","Hunt","Petty","Hubbard","Lang","Daniel","Calderon","Adkins","Pearson","Berg",
            "Gibson","Mata","Fry","Houst","Jimenez","Huff","Combs","Hogan","Padilla","Escobar","Bird","Bautista",
            "Whitaker","Jackson","Mercer","Barnett","David","Collins","Farrell","Elliott","Macias","Harris",
            "Harvey","Savage","Leon","Bentle","Preston","Patterson","Ashley","Robles","Frye","Hale","Oneill","Wyatt","Li","Baxt","Montes","Rubio",
            "Carpent","Mccoy","Horn","Levy","Hurst","Morriso","Booth","Brandt","Costa","Woodwar","Frank","Underwood",
            "Clayton","May","Hendricks","Valencia","Dickson","Watkins","Kelly","Acevedo","Stokes","Garza",
            "Parrish","Austin","Rollins","Wagner","Mcintosh","Hodge","Snow","Mckay","Rhodes","Melton","Heath","Frazier",
            "Lynch","Hartman"};

            
            string[] droneModels = { "Yuneec H520", "DJI Mavic 2 Pro", "DJI Phantom 4", "Flyability Elios" };


            /// <summary>
            ///  initialize and adds to list 6 base stations
            /// </summary>
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Ramat Aviv Gimmel",
                Lattitude = 32.124133402626434,                ,
                Longitude = 34.79810381504321,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Jaffa Port",
                Lattitude = 32.05316055248277,
                Longitude = 34.751583585054114,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Ha-Medina Square",
                Lattitude = 32.087062438465516,
                Longitude = 34.78989506663188,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Kiryat Shalom",
                Lattitude = 32.04242153854747,
                Longitude = 34.77338821800359,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Tel Aviv University",
                Lattitude = 32.11124999207207,
                Longitude = 34.80313039968891,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Neve Tzedek",
                Lattitude = 32.0640775421612,
                Longitude = 34.76549682772839,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "City Center",
                Lattitude = 32.07761654225346,
                Longitude = 34.77430851985402,
                NumOfSlots = 20,
            });


            /// <summary>
            ///  initialize and adds to list 20 drones
            /// </summary>
            int j = 20;
            for (int i = 0; i < j; i++)
            {
                Drones.Add(new Drone
                {
                    Id = rnd.Next(2000, 9999),
                    MaxWeight = (WeightCategories)rnd.Next(1, 4),
                    Model = droneModels[rnd.Next(0, 4)],
                });
            }

            /// <summary>
            ///  initialize and adds to list 20 customers
            ///  10 locations are in north part of tel aviv
            ///  11 locations are in sout part of tel aviv
            /// </summary>
            j = 10;
            for (int i = 0; i < j; i++)
            {
                Customers.Add(new Customer
                {
                    Id = rnd.Next(10000000, 100000000),
                    Name = firstNames[rnd.Next(firstNames.Length)] + " " + lastNames[rnd.Next(lastNames.Length)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = 34.751306352418034 + randCoordinates2[rnd.Next(10)],
                    Lattitude = 32.030141048120105 + randCoordinates[rnd.Next(10)],

                });;

            }
            for (int i = 0; i < j; i++)
            {
                Customers.Add(new Customer
                {
                    Id = rnd.Next(10000000, 100000000),
                    Name = firstNames[rnd.Next(firstNames.Length)] + " " + lastNames[rnd.Next(lastNames.Length)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = 34.751306352418034 + randCoordinates2[rnd.Next(10)],
                    Lattitude = 32.08274520059217 + randCoordinates[rnd.Next(10)],
                });

            }

            /// <summary>
            ///  initialize and adds to list 10 unlinked parcels
            /// </summary>
            j = 10;
            for (int i = 0; i < j; i++)
            {
                int num = rnd.Next(0, Customers.Count()-1);
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[num].Id,
                    TargetId = Customers[num+1].Id,
                    Weight = (WeightCategories)rnd.Next(1, 4),
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    DroneId = 0,

                });


            }


            /// <summary>
            ///  initialize and adds to list 5 linked to drone parcels
            /// </summary>
            for (int k = 0; k < 5; k++)
            { 
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, (int)(Customers.Count()/2))].Id,
                    TargetId = Customers[rnd.Next(((int)(Customers.Count() / 2)+1), Customers.Count())].Id,
                    DroneId = Drones[k].Id,
                    Weight = (WeightCategories)Drones[k].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now.AddMinutes(rnd.Next(60, 180)),

                });
            }

            /// <summary>
            ///  initialize and adds to list 5 pickedup by drone parcels
            /// </summary>
            for (int k = 0; k < 5; k++)
            {
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, (int)(Customers.Count() / 2))].Id,
                    TargetId = Customers[rnd.Next(((int)(Customers.Count() / 2) + 1), Customers.Count())].Id,
                    DroneId = Drones[k + 5].Id,
                    Weight = (WeightCategories)Drones[k + 5].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now.AddMinutes(rnd.Next(60, 180)),
                    PickedUp = DateTime.Now.AddMinutes(rnd.Next(180, 500)),
                });
            }

            /// <summary>
            ///  initialize and adds to list 5 delivered parcels
            /// </summary>
            for (int k = 0; k < 5; k++)
            {
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, (int)(Customers.Count() / 2))].Id,
                    TargetId = Customers[rnd.Next(((int)(Customers.Count() / 2) + 1), Customers.Count())].Id,
                    DroneId = Drones[k + 10].Id,
                    Weight = (WeightCategories)Drones[k + 10].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = DateTime.Now,
                    Scheduled = DateTime.Now.AddMinutes(rnd.Next(60, 180)),
                    PickedUp = DateTime.Now.AddMinutes(rnd.Next(180, 500)),
                    Delivered = DateTime.Now.AddMinutes(rnd.Next(500, 680)),
                });
            }
        }
    }
}

