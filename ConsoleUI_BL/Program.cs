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
            string str = "";
            do
            {
                try
                {
                    System.Console.WriteLine("Select your choice:\n"
                    + "add - add a new entity\n"
                    + "update - update an entitie's details\n"
                    + "single - print details of a single entity\n"
                    + "list - print details of a list of entities\n"
                    + "action - execute actions with entities \n"
                    + "exit - end program\n");

                    str = Console.ReadLine();
                    switch (str)
                    {
                        case "add":
                            {
                                addNewEntityMenu(data);
                                break;
                            }
                        case "update":
                            {
                                updateEntityDetailsMenu(data);
                                break;
                            }
                        case "single":
                            {
                                printSingleEntityMenu(data);
                                break;
                            }
                        case "list":
                            {
                                printEntityListsMenu(data);
                                break;
                            }
                        case "action":
                            {
                                actionsWithEntitiesMenu(data);
                                break;
                            }
                        case "exit":
                            {
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Wrong selection, restart the process.\n");
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {

                    string msg = ex.Message;
                    while (ex.InnerException != null)
                    {
                        msg += ex.InnerException.Message;
                        ex = ex.InnerException;
                    }
                    Console.WriteLine(msg);
                }

            } while (str != "exit");
        }

        private static void addNewEntityMenu(IBL.IBL data)
        {
            int num1;
            string str;
            System.Console.WriteLine("Select your choice:\n"
                + "station - add a new Base Station\n"
                + "drone- add a new Drone\n"
                + "customer - add a new Customer\n"
                + "parcel - add a new  Parcel\n");

            str = System.Console.ReadLine();

            switch (str)
            {
                case "station":
                    {
                        data.AddBaseStation(inputBaseStation());
                        break;
                    }
                case "drone":
                    {
                        Drone temp = new Drone { Parcel = null };
                        temp = inputDrone();
                        Console.WriteLine("Enter Choose Base station Id out of list for initial charge\n");
                        printBaseStations(data.GetAllAvailablBaseStations());
                        int.TryParse(Console.ReadLine(), out num1);
                        data.AddDrone(temp, num1);
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
        }

        private static void updateEntityDetailsMenu(IBL.IBL data)
        {
            int input1, input2;
            string choice, str1, str2;
            System.Console.WriteLine("select your choice:\n" +
                "base station - update a base stations details\n" +
                "drone - update a drones details \n" +
                "customer - update a customers details\n");

            choice = System.Console.ReadLine();
            switch (choice)
            {
                case "base station":
                    {
                        Console.WriteLine("Enter the Base Station's id");
                        int.TryParse(Console.ReadLine(), out input1);
                        Console.WriteLine("Enter the Base Station's name, or enter to skip");
                        str1 = Console.ReadLine();
                        Console.WriteLine("Enter the Base Station's total charging slots, or enter to skip");
                        int.TryParse(Console.ReadLine(), out input2);
                        data.UpdateBaseStation(input1, input2, str1);
                        break;
                    }
                case "drone":
                    {
                        Console.WriteLine("Enter the Drone's id");
                        int.TryParse(Console.ReadLine(), out input1);
                        Console.WriteLine("Enter the Drone's model");
                        str1 = Console.ReadLine();
                        data.UpdateDrone(input1, str1);
                        break;
                    }
                case "customer":
                    {
                        Console.WriteLine("Enter the Customer's id");
                        int.TryParse(Console.ReadLine(), out input1);
                        Console.WriteLine("Enter the Customer's name, or enter to skip");
                        str1 = Console.ReadLine();
                        Console.WriteLine("Enter the Customer's phone, or enter to skip");
                        str2 = Console.ReadLine();
                        data.UpdateCustomer(input1, str2, str1);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Wrong selection, restart the process.\n");
                        break;
                    }

            }
        }
        private static void printSingleEntityMenu(IBL.IBL data)
        {
            int input;
            string choice;
            System.Console.WriteLine("select your choice:\n" +
                             "base station - show a BaseStation's information\n" +
                             "drone - show a Drone's information\n" +
                             "customer - show a Customer's information\n" +
                             "parcel - show a Parcel's information\n");

            choice = System.Console.ReadLine();

            /// get functions return copy of specific object identified by user input
            /// details are printed to user
            switch (choice)
            {
                case "base station":
                    {
                        Console.WriteLine("Enter the Base Station's id");
                        int.TryParse(Console.ReadLine(), out input);
                        Console.WriteLine(data.GetBaseStation(input).ToString());
                        break;
                    }
                case "drone":
                    {
                        Console.WriteLine("Enter the Drone's id");
                        int.TryParse(Console.ReadLine(), out input);
                        Console.WriteLine(data.GetDrone(input).ToString());
                        break;
                    }
                case "customer":
                    {
                        Console.WriteLine("Enter the Customer's id");
                        int.TryParse(Console.ReadLine(), out input);
                        Console.WriteLine(data.GetCustomer(input).ToString());
                        break; ;
                    }
                case "parcel":
                    {
                        Console.WriteLine("Enter the Parcel's id");
                        int.TryParse(Console.ReadLine(), out input);
                        Console.WriteLine(data.GetParcel(input).ToString());
                        break;

                    }
                default:
                    {
                        Console.WriteLine("Wrong selection, restart the process.\n");
                        break;
                    }
            }
        }
        private static void printEntityListsMenu(IBL.IBL data)
        {
            string str;
            System.Console.WriteLine("select your choice:\n" +
             "stations - show Base Stations list \n" +
             "drones - show Drones list\n" +
             "customers - show Customers list\n" +
             "parcels - show Parcels list\n" +
             "unlinked - show unlinked parcels list\n" +
             "charge - show Base Statios with available charging slots list\n");

            str = System.Console.ReadLine();

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


        }
        private static void actionsWithEntitiesMenu(IBL.IBL data)
        {
            string choice;
            int input1, input2;
            Console.WriteLine("Select your choice:\n" +
            "charge -send drone to charge\n" +
            "discharge - release a drone from charge\n" +
            "link - link a drone to  a parcel\n" +
            "pickup - pickup parcel by its linked drone from sender\n" +
            "deliver - deliver parcel to target\n");

            choice = System.Console.ReadLine();
            switch (choice)
            {
                case "charge":
                    {
                        Console.WriteLine("Enter drone ID: ");
                        if (int.TryParse(Console.ReadLine(), out input1))
                            data.ChargeDrone(input1);
                        break;
                    }
                case "discharge":
                    {
                        Console.WriteLine("Enter drone ID: ");
                        int.TryParse(Console.ReadLine(), out input1);
                        Console.WriteLine("Enter time of charge in hours: ");
                        int.TryParse(Console.ReadLine(), out input2);
                        data.DischargeDrone(input1, input2);
                        break;
                    }
                case "link":
                    {
                        Console.WriteLine("Enter drone ID: ");
                        int.TryParse(Console.ReadLine(), out input1);
                        data.LinkDroneToParcel(input1);
                        break;
                    }
                case "pickup":
                    {
                        Console.WriteLine("Enter drone ID: ");
                        int.TryParse(Console.ReadLine(), out input1);
                        data.DroneParcelPickUp(input1);
                        break;
                    }
                case "deliver":
                    {
                        Console.WriteLine("Enter drone ID: ");
                        int.TryParse(Console.ReadLine(), out input1);
                        data.DroneParcelDelivery(input1);
                        break;
                    }
                default:
                    break;
            }


        }

        /// <summary>
        /// create and get user input for new base station
        /// </summary>
        /// <returns> Base Station object</returns>
        /// 

        private static BaseStation inputBaseStation()
        {
            BaseStation station = new BaseStation { StationLocation = new Location(), };
            int num;
            double x;
            bool flag;
            do
            {
                flag = true;
                Console.WriteLine("enter a 4 digit number of Base Station's id"); 
                try
                {
                    if (!(int.TryParse(Console.ReadLine(), out num)))
                        throw new Exception("enter digits only!");
                    if(num < 1000 || num >= 10000)
                        throw new Exception("ID not in range!");
                    station.Id = num;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }
            } while (flag==false);
            Console.WriteLine("enter Base Station's name");
            station.Name = Console.ReadLine();
            do
            {
                flag = true;
                Console.WriteLine("enter Base Station's longtitude coordinate \n Range between 34.745 - 34.808 ");
                try
                {            
                    if(!(double.TryParse(Console.ReadLine(), out x)))
                        throw new Exception("enter digits only!");
                    if(x < 34.745 || x > 34.808)
                        throw new Exception("coordinate out of range!");
                    station.StationLocation.Longtitude = x;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }

            } while (flag==false);

            do
            {
                flag = true;
                Console.WriteLine("enter Base Station's lattitude coordinate \n Range between 32.033 - 32.127 ");
                try
                {
                    if (!(double.TryParse(Console.ReadLine(), out x)))
                        throw new Exception("enter digits only!");
                    if (x < 32.033 || x > 32.127)
                        throw new Exception("coordinate out of range!");
                    station.StationLocation.Lattitude = x;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }

            } while (flag == false);

            do
            {
                flag = true;
                Console.WriteLine("enter Base Station's number of available charging slots");
                try
                {
                    if(!(int.TryParse(Console.ReadLine(), out num)))
                        throw new Exception("enter digits only!");
                    if (num < 0)
                        throw new Exception("positive number only!");
                    station.NumOfSlots = num;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }
            } while ( flag==false);
           
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
            bool flag; 
            do
            {
                flag = true;
                Console.WriteLine("enter a 4 digit number for Drone's id , first digit must be 2 or larger");
                try
                {
                    if (!int.TryParse(Console.ReadLine(), out num))
                        throw new Exception("enter digits only!");
                    if (!(num >= 2000 && num < 10000))
                        throw new Exception("id not in range!");
                    dr.Id = num;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = false;
                }
            } while (flag == false);

            Console.WriteLine("enter Drone's model");
            dr.Model = Console.ReadLine();

            do
            {
                flag = true;
                Console.WriteLine("enter Drone's Max Weight category\nLight: 1   Medium: 2   Heavy: 3 ");
                try
                {
                    if (!(int.TryParse(Console.ReadLine(), out num) && (num == 1 || num == 2 || num == 3)))
                        throw new Exception("enter one of optional choices only!");
                    dr.MaxWeight = (WeightCategories)num;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = false;
                }

            } while (flag == false);
            dr.Parcel = null;
            return dr;
        }
     

        /// <summary>
        /// create and get user input for new customer
        /// </summary>
        /// <returns> Customer object</returns>
        private static Customer inputCustomer()
        {

            Customer person = new Customer { CustomerLocation = new Location(), };
            int num;
            double x;
            bool flag;
            do
            {
                flag = true;
                Console.WriteLine("enter a 8 digit number for Customer's id");
                try
                {
                    flag = true;
                    if (!int.TryParse(Console.ReadLine(), out num))
                        throw new Exception("enter digits only!");
                    if (!(num >= 10000000 && num < 100000000))
                        throw new Exception("id not in range!");
                    person.Id = num;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = false;
                }
            } while (flag == false);

            Console.WriteLine("enter Customer's name");
            person.Name = Console.ReadLine();
            do
            {
                flag = true;
                Console.WriteLine("enter Customers's Phone Number");
                try
                {
                    person.Phone = Console.ReadLine();
                    if (!int.TryParse(person.Phone, out num))
                        throw new Exception("enter digits only!");
                    if (!(person.Phone.Length == 10 ))
                        throw new Exception("phone number must be 10 digits long!");
                    person.Id = num;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = false;
                }
            } while (flag == false);

            do
            {
                flag = true;
                Console.WriteLine("enter Customer's longtitude coordinate \n between 34.745 - 34.808 ");
                try
                {
                    if (!(double.TryParse(Console.ReadLine(), out x)))
                        throw new Exception("enter digits only!");
                    if (!(x > 34.745 && x < 34.808))
                        throw new Exception("coordinate out of range!");
                    person.CustomerLocation.Longtitude = x;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = false;
                }

            } while (flag == false);

            do
            {
                flag = true;
                Console.WriteLine("enter customers's lattitude coordinate \n  Range between 32.033 - 32.127");
                try
                {
                    if(!(double.TryParse(Console.ReadLine(), out x)))
                        throw new Exception("enter digits only!");
                    if (! (x > 32.033 && x < 32.127))
                        throw new Exception("coordinate out of range!");
                    person.CustomerLocation.Lattitude = x;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    flag = false;
                }
               
            } while (flag == false);
          
            return person;
        }
        private static Parcel inputParcel()
        {
            Parcel package = new Parcel { Sender = new CustomerInParcel(), Target = new CustomerInParcel() };
            int num;
            bool flag;
            do
            {
                flag = true;
                Console.WriteLine("Enter an 8 digit number for Parcel's sender id");
                try
                {
                    if (!(int.TryParse(Console.ReadLine(), out num)))
                        throw new Exception("enter digits only!");
                    if(num < 10000000 ||num >= 100000000)
                        throw new Exception("ID not in range!");
                    package.Sender.Id = num;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }
                
            } while (flag==false); 
           
            do
            {
                flag = true;
                Console.WriteLine("Enter an 8 digit number for Parcel's target id");
                try
                {
                    if (!(int.TryParse(Console.ReadLine(), out num)))
                        throw new Exception("enter digits only!");
                    if (num < 10000000 || num >= 100000000)
                        throw new Exception("ID not in range!");
                    package.Target.Id = num;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }
            } while (flag==false);
          
            do
            {
                flag = true;
                Console.WriteLine("Enter Parcel's Weight category \n light: 1   medium: 2   heavy:3 ");
                try
                {
                    if (!(int.TryParse(Console.ReadLine(), out num) && (num == 1 || num == 2 || num == 3)))
                        throw new Exception("Enter one of optional choices only!");
                    package.Weight = (WeightCategories)num;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }
            } while (flag==false);
            
            do
            {
                flag = true;
                Console.WriteLine("Enter Parcel's Priority category \nRegular: 1  Express: 2  Emergency: 3 ");
                try
                {
                    if(!(int.TryParse(Console.ReadLine(), out num) && (num == 1 || num == 2 || num == 3)))
                        throw new Exception("Enter one of optional choices only!");
                    package.Priority = (Priority)num;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    flag = false;
                }
            } while (flag==false);
           
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
