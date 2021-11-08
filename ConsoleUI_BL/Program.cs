using System;
using System.Collections.Generic;
using IBL;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL.IBL data = new BL.BL();
            string str;
            int num1, num2;
            do
            {
                System.Console.WriteLine("select your choice:\n"
                    + "add - add a new object\n"
                    + "update - update an object\n"
                    + "show - show details of object\n" + "list - show details of array\n"
                    + "distance - get distance from coordinates to base station or customer location\n"
                    + "exit - exit\n");

                str = System.Console.ReadLine();
                switch (str)
                {
                    case "add":
                        {
                            System.Console.WriteLine("Select your choice:\n"
                                                        + "station - add a new Base Station\n"
                                                        + "drone- add a new Drone\n"
                                                        + "customer - add a new Customer\n"
                                                        + "parcel - add a new  Parcel\n");

                            str = System.Console.ReadLine();

                            /// input function return, a  specified user inputed object, that is sent to 
                            /// add function to append to the appropriate database.
                            switch (str)
                            {
                                case "station":
                                    {
                                        data.AddBaseStation(InputBaseStation());
                                        break;
                                    }
                                case "drone":
                                    {
                                        Drone temp = InputDrone();
                                        Console.WriteLine("Enter Base station Id for initial charge\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        data.AddDrone(temp ,num1);
                                        Console.WriteLine("Enter Base station Id for initial charge\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        break;
                                    }
                                case "customer":
                                    {

                                        break;
                                    }
                                case "parcel":
                                    {
                                        break;
                                        
                                    }


                                default:
                                    {
                                        Console.WriteLine("Wrong selection, restart the process.\n");
                                        break;
                                    }
                            }
                            break;
                        }
                }

            } while (str != "exit");
        }

        /// <summary>
        /// create and get user input for new base station
        /// </summary>
        /// <returns> Base Station object</returns>
         private static BaseStation InputBaseStation()
        {
            BaseStation station = new BaseStation();
            int num;
            double x;
            Console.WriteLine("enter Base Station's id");
            if (int.TryParse(Console.ReadLine(), out num))
                station.Id = num;
            Console.WriteLine("enter Base Station's name");
            station.Name = Console.ReadLine();
            Console.WriteLine("enter Base Station's longtitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                station.StationLocation.Longtitude = x;
            Console.WriteLine("enter Base Station's lattitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                station.StationLocation.Lattitude = x;
            station.NumOfSlots = 10;
            return station;
        }

        /// <summary>
        /// create and get user input for new drone
        /// </summary>
        /// <returns> Drone object</returns>
        private static Drone InputDrone()
        {
            Drone dr = new Drone();
            int num;
            double x;
            Console.WriteLine("Enter Drone's id");
            if (int.TryParse(Console.ReadLine(), out num))
                dr.Id = num;
            Console.WriteLine("Enter Drone's model");
            dr.Model = Console.ReadLine();
            Console.WriteLine("Enter Drone's Max Weight category" +
                                "1: light   2: medium   3: heavy ");
            if (int.TryParse(Console.ReadLine(), out num))
                dr.MaxWeight = (WeightCategories)num;
            return dr;
        }

        /// <summary>
        /// create and get user input for new customer
        /// </summary>
        /// <returns> Customer object</returns>
        private static Customer InputCustomer()
        {
            Customer person = new Customer();
            int num;
            double x;
            Console.WriteLine("enter Customer's id");
            if (int.TryParse(Console.ReadLine(), out num))
                person.Id = num;
            Console.WriteLine("Enter Customer's name");
            person.Name = Console.ReadLine();
            Console.WriteLine("Enter Customers's Phone Number");
            person.Phone = Console.ReadLine();
            return person;
        }

        /// <summary>
        /// print information of a list of base stations
        /// </summary>
        /// <param name="stations"> IEnumerable<BaseStation> type </param>
        private static void PrintBaseStations(IEnumerable<BaseStation> stations)
        {
            Console.WriteLine("Base Stations List:\n");
            foreach (BaseStation stn in stations)
            {
                Console.WriteLine(stn);
            }
        }

        /// <summary>
        /// print information of a list of drones
        /// </summary>
        /// <param name="drones"> IEnumerable<Drone> type </param>
        public static void printDrones(IEnumerable<Drone> drones)
        {
            Console.WriteLine("Drones List:\n");
            foreach (Drone dr in drones)
            {
                Console.WriteLine(dr);
            }
        }

        /// <summary>
        /// print information of a list of customers
        /// </summary>
        /// <param name="customers"> IEnumerable<Customer> type </param>
        public static void printCustomers(IEnumerable<Customer> customers)
        {
            Console.WriteLine("Customers List:\n");
            foreach (Customer cst in customers)
            {
                Console.WriteLine(cst);
            }
        }

        /// <summary>
        /// print information of a list of parcels
        /// </summary>
        /// <param name = "parcels"> IEnumerable<Parcel> type </param>
        public static void printParcels(IEnumerable<Parcel> parcels)
        {
            Console.WriteLine("Parcels List:\n");
            foreach (Parcel prcl in parcels)
            {
                Console.WriteLine(prcl);
            }
        }

        /// <summary>
        /// print details of a single base station
        /// </summary>
        /// <param name="st"> base object station to print</param>
        public static void PrintBaseStation(BaseStation st)
        {
            Console.WriteLine("Base station details:\n");
            Console.WriteLine(st);
        }
        /// <summary>
        /// print details of a single drone
        /// </summary>
        /// <param name="dr"> drone object to print</param>
        public static void PrintDrone(Drone dr)
        {
            Console.WriteLine("Drone details:\n");
            Console.WriteLine(dr);
        }
        /// <summary>
        /// print details of a single Customer
        /// </summary>
        /// <param name="cstmr"> customer object to print</param>
        public static void PrintCustomer(Customer cstmr)
        {
            Console.WriteLine("Customer  details:\n");
            Console.WriteLine(cstmr);
        }
        /// <summary>
        /// print details of a single parcel
        /// </summary>
        /// <param name="prc"> parcel object to print</param>
        public static void PrintParcel(Parcel prc)
        {
            Console.WriteLine("Base station details:\n");
            Console.WriteLine(prc);
        }


    }
   
}
