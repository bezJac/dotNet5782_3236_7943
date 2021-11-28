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
         double[] coordinates = { 31.76010458735114, 35.17183547118651,31.81262202251814, 35.23009927060373,
                                         31.80828968961037, 35.2171112986503,31.764645582668795, 35.21346981118673,
                                         31.763200744691808, 35.18810078185715,31.774655626139023, 35.190407057250745,
                                         31.779092721598428, 35.21346981118673,31.784870945390598, 35.19259194972889,
                                         31.812003130257516, 35.242480327979884,31.812415725558708, 35.20363779503508,
                                         31.80096552144744, 35.225729484788474,31.792093216890663, 35.19162088554631,
                                         31.790132941712947, 35.2246370385494,31.782497789074736, 35.18045365666398,
                                         31.782910516122005, 35.22403012331144,31.767741587132058, 35.18761524867568,
                                         31.765161589813747, 35.21431949007523,31.755253731649855, 35.18312408080393,
                                         31.752673386189688, 35.21711129713064,31.737293035664848, 35.19744726482733,};


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
                Name = "Pisgat Ze'ev East",
                Lattitude = 31.818565506554837,
                Longitude = 35.24313869573931,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "City Center",
                Lattitude = 31.782102992801367,
                Longitude = 35.21990315275638,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Hebrew University - Giv'at Ram",
                Lattitude = 31.777450996979468,
                Longitude = 35.19706750232356,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Ramat Eshkol",
                Lattitude = 31.802109766530826,
                Longitude = 35.22328877726481,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Gilo",
                Lattitude = 31.73367746434541,
                Longitude = 35.1878493660773,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Talpiot",
                Lattitude = 31.75207025547218,
                Longitude = 35.22585387261875,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Giv'at Masu'a",
                Lattitude = 31.750816286403072,
                Longitude = 35.16746927212728,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Ramot",
                Lattitude = 31.81691522418518,
                Longitude = 35.20022983512685,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Bayit Vagan",
                Lattitude = 31.750816286403072,
                Longitude = 35.16746927212728,
                NumOfSlots = 20,
            });
            Stations.Add(new BaseStation
            {
                Id = rnd.Next(1000, 10000),
                Name = "Har Nof",
                Lattitude = 31.783813337113397,
                Longitude = 35.174775551705864,
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
            j = 20;
            for (int i = 0,l=0; i < j; i++,l+=2)
            {
                Customers.Add(new Customer
                {
                    Id = rnd.Next(10000000, 100000000),
                    Name = firstNames[rnd.Next(firstNames.Length)] + " " + lastNames[rnd.Next(lastNames.Length)],
                    Phone = $"0{ rnd.Next(50, 58)}{ rnd.Next(1000000, 9999999)}",
                    Longitude = coordinates[l+1],
                    Lattitude = coordinates[l],

                });;

            }
            

            /// <summary>
            ///  initialize and adds to list 10 unlinked parcels
            /// </summary>
            j = 10;
            for (int i = 0; i < j; i++)
            {
                int num = rnd.Next(0, Customers.Count-1);
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

            DateTime time = DateTime.Now.Subtract(new TimeSpan(4, 0, 0));
            /// <summary>
            ///  initialize and adds to list 5 linked to drone parcels
            /// </summary>
            for (int k = 0; k < 5; k++)
            { 
                Parcels.Add(new Parcel
                {
                    Id = ++Config.RunIdParcel,
                    SenderId = Customers[rnd.Next(0, (int)(Customers.Count/2))].Id,
                    TargetId = Customers[rnd.Next(((int)(Customers.Count / 2)+1), Customers.Count)].Id,
                    DroneId = Drones[k].Id,
                    Weight = (WeightCategories)Drones[k].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = time,
                    Scheduled = time.AddMinutes(rnd.Next(60, 180)),

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
                    SenderId = Customers[rnd.Next(0, (int)(Customers.Count / 2))].Id,
                    TargetId = Customers[rnd.Next(((int)(Customers.Count / 2) + 1), Customers.Count)].Id,
                    DroneId = Drones[k + 5].Id,
                    Weight = (WeightCategories)Drones[k + 5].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = time,
                    Scheduled = time.AddMinutes(rnd.Next(60, 180)),
                    PickedUp = time.AddMinutes(rnd.Next(180, 500)),
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
                    SenderId = Customers[rnd.Next(0, (int)(Customers.Count / 2))].Id,
                    TargetId = Customers[rnd.Next(((int)(Customers.Count / 2) + 1), Customers.Count)].Id,
                    DroneId = Drones[k + 10].Id,
                    Weight = (WeightCategories)Drones[k + 10].MaxWeight,
                    Priority = (Priorities)rnd.Next(1, 4),
                    Requested = time,
                    Scheduled = time.AddMinutes(rnd.Next(60, 180)),
                    PickedUp = time.AddMinutes(rnd.Next(180, 500)),
                    Delivered = time.AddMinutes(rnd.Next(500, 680)),
                });
            }
        }
    }
}

