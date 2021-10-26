using System;
using System.Collections.Generic;
using DalObject;
using IDAL.DO;

namespace ConsoleUI
{
    class Program
    {

        static void Main(string[] args)

        {
            DalObject.DalObject data = new DalObject.DalObject();
            string str;
            int id1, id2;
            do
            {
                System.Console.WriteLine("select your choice:\n" 
                    + "add - add a new object\n" 
                    + "update - update an object\n"
                    + "show - show details of object\n" + "list - show details of array\n"
                    + "distance - get distance from coordinates to base station or customer location\n"
                    + "exit - exit\n"  );

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

                        // input function return specified, user inputed object, that is sent to 
                        // add function to append to the appropriate database.
                        switch (str)
                        {
                            case "station":
                            {
                                data.addBaseStation(inputBaseStation());
                                break;
                            }
                            case "drone":
                            {
                                data.addDrone(inputDrone());
                                break;
                            }
                            case "customer":
                            {
                                data.addCustomer(inputCustomer());
                                break;
                            }
                            case "parcel":
                            {
                                data.addParcel(inputParcel());
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
                    case "update":
                    {
                        System.Console.WriteLine("select your choice:\n" +
                                            "link - Link Parcel to Drone\n" +
                                            "pickup - Pickup Parcel by Drone\n" +
                                            "delivery - Deliver Parcel to Customer\n" +
                                            "charge - Send Drone to charging station\n" +
                                            "discharge - Release Drone from charging station\n");

                        str = System.Console.ReadLine();

                        // get functions return copy of specific object identified by user input
                        // and is sent to modification functions for update and replacment 
                        // of old object in database/s.
                        switch (str)
                        {
                            case "link":
                            {
                                Console.WriteLine("Enter Parcel's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine("Enter Drone's id\n");
                                int.TryParse(Console.ReadLine(), out id2);
                                data.linkParcelToDrone(data.GetParcel(id1), data.GetDrone(id2));
                                break;
                            }
                            case "pickup":
                            {
                                Console.WriteLine("Enter Parcel's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                data.droneParcelPickup(data.GetParcel(id1));
                                break;
                            }
                            case "delivery":
                            {
                                Console.WriteLine("Enter Parcel's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                data.parcelDelivery(data.GetParcel(id1));
                                break;
                            }
                            case "charge":
                            {
                                Console.WriteLine("Enter Base station's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine("Enter Drone's id\n");
                                int.TryParse(Console.ReadLine(), out id2);
                                data.chargeDrone(data.GetBaseStation(id1), data.GetDrone(id2));
                                break;
                            }
                            case "discharge":
                            {
                                Console.WriteLine("Enter Base station's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine("Enter Drone's id\n");
                                int.TryParse(Console.ReadLine(), out id2);
                                data.chargeDrone(data.GetBaseStation(id1), data.GetDrone(id2));
                                data.releaseDroneCharge(data.GetBaseStation(id1), data.GetDrone(id2));
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

                        // get functions return copy of specific object identified by user input
                        // details are printed to user
                        switch (str)
                        {
                            case "base station":
                            {
                                Console.WriteLine("Enter the Base Station's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine(data.GetBaseStation(id1).ToString());
                                break;
                            }
                            case "drone":
                            {
                                Console.WriteLine("Enter the Drone's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine(data.GetDrone(id1).ToString());
                                break;
                            }
                            case "customer":
                            {
                                Console.WriteLine("Enter the Customer's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine(data.GetCustomer(id1).ToString());
                                break; ;
                            }
                            case "parcel":
                            {
                                Console.WriteLine("Enter the Parcel's id\n");
                                int.TryParse(Console.ReadLine(), out id1);
                                Console.WriteLine(data.GetParcel(id1).ToString());
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

                        // get functions return a  copy of a list of objects
                        // the copy is sent to print function to print for user
                        switch (str)
                        {
                            case "stations":
                            {
                                printBaseStations(data.GetAllBaseStations());
                                break;
                            }
                            case "drones":
                            {
                                printDrones(data.GetAllDrones());
                                break;
                            }
                            case "customers":
                            {
                                printCustomers(data.GetAllCustomers());
                                break;
                            }
                            case "parcels":
                            {
                                printParcels(data.GetAllParcels());
                                break;
                            }
                            case "unlinked":
                            {
                                printParcels(data.GetUnlinkedParcels());
                                break;
                            }
                            case "charge":
                            {    
                                printBaseStations(data.GetAvailableCharge());
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
                    case "distance":
                    {
                    
                        double longt, latt;
                        Console.WriteLine("enter longtitude coordinate:");
                        double.TryParse(Console.ReadLine(), out longt);
                        System.Console.WriteLine("enter lattitude coordinate:");
                        double.TryParse(Console.ReadLine(), out latt);
                        Console.WriteLine("select your choice:\t"+ "station/customer");
                        str = Console.ReadLine();
                        switch(str)
                        {
                        case "station":
                        {
                            Console.WriteLine("enter station's id:");
                            int.TryParse(Console.ReadLine(), out id1);
                            Console.WriteLine($"Distance is: {Math.Round(data.GetDistance(longt, latt, data.GetBaseStation(id1)), 3)} KM.\n");
                            break;
                        }
                        case "customer":
                        {
                            Console.WriteLine("enter customer's id:");
                            int.TryParse(Console.ReadLine(), out id1);
                            Console.WriteLine($"Distance is: {Math.Round(data.GetDistance(longt, latt, data.GetCustomer(id1)),3)} KM\n");
                            break;
                        }

                            }

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
            } while (str != "exit");

        }

        // returns Base Station object initialized by user input
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
                station.Longitude = x;
            Console.WriteLine("enter Base Station's lattitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                station.Lattitude = x;
            Console.WriteLine("enter Base Station's number of charging slots");
            if (int.TryParse(Console.ReadLine(), out num))
                station.NumOfSlots = num;
            return station;
        }

        // returns drone object initialized by user input
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
            Console.WriteLine("Enter Drone's Status" +
                                "1: available   2: maintenance   3: delivery ");
            if (int.TryParse(Console.ReadLine(), out num))
                dr.Status = (DroneStatus)num;
            Console.WriteLine("Enter Drone's battery level");
            if (double.TryParse(Console.ReadLine(), out x))
                dr.Battery = x;
            return dr;
        }

        // returns Customer object initialized by user input
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
            person.Name = Console.ReadLine();
            Console.WriteLine("Enter Base Station's longtitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                person.Longitude = x;
            Console.WriteLine("Enter Base Station's lattitude coordinate");
            if (double.TryParse(Console.ReadLine(), out x))
                person.Lattitude = x;
            return person;
        }

        // returns Parcel object initialized by user input
        private static Parcel inputParcel()
        {
            Parcel package = new Parcel();
            int num;
            Console.WriteLine("Enter Parcel's sender id");
            if (int.TryParse(Console.ReadLine(), out num))
                package.SenderId = num;
            Console.WriteLine("Enter Parcel's target id");
            if (int.TryParse(Console.ReadLine(), out num))
                package.TargetId = num;
            Console.WriteLine("Enter Parcel's Weight category" +
                                "1: light   2: medium   3: heavy \n");
            if (int.TryParse(Console.ReadLine(), out num))
                package.Weight = (WeightCategories)num;
            package.Requested = DateTime.Now;
            return package;
        }
       
        // prints list of base stations
        private static void printBaseStations(List<BaseStation> stations)
        {
            foreach (BaseStation stn in stations)
            {
                Console.WriteLine(stn.ToString());
            }
        }

        // prints list of drones
        public static void printDrones(List<Drone> drns)
        {
            foreach (Drone dr in drns)
            {
                Console.WriteLine(dr.ToString());
            }
        }

        // prints list of Customers
        public static void printCustomers(List<Customer> customers)
        {
            foreach (Customer cst in customers)
            {
                Console.WriteLine(cst.ToString());
            }
        }

        // prints list of Parcels
        public static void printParcels(List<Parcel> parcels)
        {
            foreach (Parcel prcl in parcels)
            {
                Console.WriteLine(prcl.ToString());
            }
        }


    }
}

