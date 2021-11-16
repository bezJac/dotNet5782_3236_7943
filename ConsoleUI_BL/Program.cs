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
                    + "update - update an object details\n"
                    + "show - show details of object\n" + "list - show details of array\n"
                    + "action- \n"
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
                                        data.AddBaseStation(inputBaseStation());
                                        break;
                                    }
                                case "drone":
                                    {
                                        Drone temp = inputDrone();
                                        Console.WriteLine("Enter Base station Id for initial charge\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        data.AddDrone(temp ,num1);
                                        break;
                                    }
                                case "customer":
                                    {
                                        data.AddCustomer(inputCustomer());
                                        break;
                                    }
                                case "parcel":
                                    {
                                        data.AddParcel(inputParcel());
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
                    case "show":
                        {
                            System.Console.WriteLine("select your choice:\n" +
                                "base station - show a BaseStation's information\n" +
                                "drone - show a Drone's information\n" +
                                "customer - show a Customer's information\n" +
                                "parcel - show a Parcel's information\n");

                            str = System.Console.ReadLine();

                            /// get functions return copy of specific object identified by user input
                            /// details are printed to user
                            switch (str)
                            {
                                case "base station":
                                    {
                                        Console.WriteLine("Enter the Base Station's id\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        Console.WriteLine(data.GetBaseStation(num1).ToString());
                                        break;
                                    }
                                case "drone":
                                    {
                                        Console.WriteLine("Enter the Drone's id\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        Console.WriteLine(data.GetDrone(num1).ToString());
                                        break;
                                    }
                                case "customer":
                                    {
                                        Console.WriteLine("Enter the Customer's id\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        Console.WriteLine(data.GetCustomer(num1).ToString());
                                        break; ;
                                    }
                                case "parcel":
                                    {
                                        Console.WriteLine("Enter the Parcel's id\n");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        Console.WriteLine(data.GetParcel(num1).ToString());
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
                    case "list":
                        {
                            System.Console.WriteLine("select your choice:\n" +
                                                "stations - show Base Stations list \n" +
                                                "drones - show Drones list\n" +
                                                "customers - show Customers list\n" +
                                                "parcels - show Parcels list\n" +
                                                "unlinked - show unlinked parcels list\n" +
                                                "charge - show Base Statios with available charging slots list\n");

                            str = System.Console.ReadLine();

                            /// get functions return a  copy of a list of objects
                            /// the copy is sent to print function to print for user
                            switch (str)
                            {
                                case "stations":
                                    {
                                        printBaseStations(data.GetALLBaseStationInList());
                                        break;
                                    }
                                case "drones":
                                    {
                                        printDrones(data.GetAllDronesInList());
                                        break;
                                    }
                                case "customers":
                                    {
                                        printCustomers(data.GetAllCustomersInList());
                                        break;
                                    }
                                case "parcels":
                                    {
                                        printParcels(data.GetAllParcelsInList());
                                        break;
                                    }
                                case "unlinked":
                                    {
                                        printParcels(data.GetAllUnlinkedParcels());
                                        break;
                                    }
                                case "charge":
                                    {
                                        printBaseStations(data.GetAllAvailablBaseStations());
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
                    case "action":
                            {
                            System.Console.WriteLine("Select your choice:\n"
                                                        + "charge -send drone to charge\n"
                                                        + "discharge - release a drone from charge\n"
                                                        + "link - link a drone to  a parcel\n"
                                                        + "pickup - pickup parcel by its linked drone from sender\n"
                                                        + "deliver - deliver parcel to target\n");

                            str = System.Console.ReadLine();
                            switch (str)
                            {
                                case "charge":
                                    {
                                        Console.WriteLine("Enter drone ID: ");
                                        if (int.TryParse(Console.ReadLine(), out num1))
                                            data.ChargeDrone(num1);
                                        break;
                                    }
                                case "discharge":
                                    {
                                        Console.WriteLine("Enter drone ID: ");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        Console.WriteLine("Enter time of charge in hours: ");
                                        int.TryParse(Console.ReadLine(), out num2);
                                        data.DischargeDrone(num1, num2);
                                        break; 
                                    }
                                case "link":
                                    {
                                        Console.WriteLine("Enter drone ID: ");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        data.LinkDroneToParcel(num1);
                                        break;
                                    }
                                case "pickup":
                                    {
                                        Console.WriteLine("Enter drone ID: ");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        data.DroneParcelPickUp(num1);
                                        break;
                                    }
                                case "deliver":
                                    {
                                        Console.WriteLine("Enter drone ID: ");
                                        int.TryParse(Console.ReadLine(), out num1);
                                        data.DroneParcelDelivery(num1);
                                        break;
                                    }
                                default:
                                    break;
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
         private static BaseStation inputBaseStation()
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
            Console.WriteLine("enter Base Station's number of available charging");
            if(int.TryParse(Console.ReadLine(),out num))
            station.NumOfSlots = num;
            station.DronesCharging = null;
            return station;
        }

        /// <summary>
        /// create and get user input for new drone
        /// </summary>
        /// <returns> Drone object</returns>
        private static Drone inputDrone()
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
        private static Customer inputCustomer()
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
            Console.WriteLine("Enter Customer's Longtitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                person.CustomerLocation.Longtitude=x;
            Console.WriteLine("Enter Customer's Lattitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                person.CustomerLocation.Lattitude = x;
            return person;
        }
        private static Parcel inputParcel()
        {
            Parcel package = new Parcel();
            int num;
            Console.WriteLine("Enter Parcel's sender id");
            if (int.TryParse(Console.ReadLine(), out num))
                package.Sender.Id = num;
            Console.WriteLine("Enter Parcel's target id");
            if (int.TryParse(Console.ReadLine(), out num))
                package.Target.Id = num;
            Console.WriteLine("Enter Parcel's Weight category" +
                                "1: light   2: medium   3: heavy \n");
            if (int.TryParse(Console.ReadLine(), out num))
                package.Weight = (WeightCategories)num;
            Console.WriteLine("Enter Parcel's Priority category" +
                                "1:Regular   2: Express   3: Emergency \n");
            if (int.TryParse(Console.ReadLine(), out num))
                package.Priority = (Priority)num;
            package.Ordered = DateTime.Now;
            package.Linked = DateTime.MinValue;
            package.PickedUp = DateTime.MinValue;
            package.Delivered = DateTime.MinValue;
            package.Drone = null;
            return package;
        }

        /// <summary>
        /// print information of a list of base stations
        /// </summary>
        /// <param name="stations"> IEnumerable<BaseStation> type </param>
        private static void printBaseStations(IEnumerable<BaseStationInList> stations)
        {
            Console.WriteLine("Base Stations List:\n");
            foreach (BaseStationInList stn in stations)
            {
                Console.WriteLine(stn);
            }
        }

        /// <summary>
        /// print information of a list of drones
        /// </summary>
        /// <param name="drones"> IEnumerable<Drone> type </param>
        private static void printDrones(IEnumerable<DroneInList> drones)
        {
            Console.WriteLine("Drones List:\n");
            foreach (DroneInList dr in drones)
            {
                Console.WriteLine(dr);
            }
        }

        /// <summary>
        /// print information of a list of customers
        /// </summary>
        /// <param name="customers"> IEnumerable<Customer> type </param>
        private static void printCustomers(IEnumerable<CustomerInList> customers)
        {
            Console.WriteLine("Customers List:\n");
            foreach (CustomerInList cst in customers)
            {
                Console.WriteLine(cst);
                   
            }
        }

        /// <summary>
        /// print information of a list of parcels
        /// </summary>
        /// <param name = "parcels"> IEnumerable<Parcel> type </param>
        private static void printParcels(IEnumerable<ParcelInList> parcels)
        {
            Console.WriteLine("Parcels List:\n");
            foreach (ParcelInList prcl in parcels)
            {
                Console.WriteLine(prcl);
            }
        }

        /// <summary>
        /// print details of a single base station
        /// </summary>
        /// <param name="st"> base object station to print</param>
        private static void printBaseStation(BaseStation st)
        {
            Console.WriteLine("Base station details:\n");
            Console.WriteLine(st);
        }
        /// <summary>
        /// print details of a single drone
        /// </summary>
        /// <param name="dr"> drone object to print</param>
        private static void printDrone(Drone dr)
        {
            Console.WriteLine("Drone details:\n");
            Console.WriteLine(dr);
        }
        /// <summary>
        /// print details of a single Customer
        /// </summary>
        /// <param name="cstmr"> customer object to print</param>
        private static void printCustomer(Customer cstmr)
        {
            Console.WriteLine("Customer  details:\n");
            Console.WriteLine(cstmr);
        }
        /// <summary>
        /// print details of a single parcel
        /// </summary>
        /// <param name="prc"> parcel object to print</param>
        private static void printParcel(Parcel prc)
        {
            Console.WriteLine("Base station details:\n");
            Console.WriteLine(prc);
        }
        

    }
   
}
