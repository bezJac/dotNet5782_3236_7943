﻿using System;
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
            int id1,id2;
            do
            {
                System.Console.WriteLine("select your choice:\n"
                + "add - add a new object\n"
                + "update - update an object\n"
                + "show - show details of object\n"
                + "list - show details of array\n"
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
                            switch (str)
                            {
                                case "station":
                                    {
                                        BaseStation station = new BaseStation();
                                        inputBaseStation(station);
                                        data.addBaseStation(station);
                                        break;
                                    }
                                case "drone":
                                    {
                                        Drone drn = new Drone();
                                        inputDrone(drn);
                                        data.addDrone(drn);
                                        break;
                                    }
                                case "customer":
                                    {
                                        Customer person = new Customer();
                                        inputCustomer(person);
                                        data.addCustomer(person);
                                        break;
                                    }
                                case "parcel":
                                    {
                                        Parcel package = new Parcel();
                                        inputParcel(package);
                                        data.addParcel(package);
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
                            System.Console.WriteLine("select your choice:\n"+
                                                "link - Link Parcel to Drone\n"+
                                                "pickup - Pickup Parcel by Drone\n"+
                                                "delivery - Deliver Parcel to Customer\n"+
                                                "charge - Send Drone to charging station\n"+
                                                "discharge - Release Drone from charging station\n");
                            str = System.Console.ReadLine();
                            switch (str)
                            {
                                case "link":
                                    {
                                        Console.WriteLine("Enter Parcel's id\n");
                                        int.TryParse(Console.ReadLine(), out id1);
                                        Console.WriteLine("Enter Drone's id\n");
                                        int.TryParse(Console.ReadLine(), out id2);
                                        data.linkParcelToDrone(data.GetParcel(id1),data.GetDrone(id2));
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
                                        data.chargeDrone(data.GetBaseStation(id1),data.GetDrone(id2));
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
                            System.Console.WriteLine("select your choice:\n"+
                                "base station - show a BaseStation's information\n"+
                                "drone - show a Drone's information\n"+
                                "customer - show a Customer's information\n"+
                                "parcel - show a Parcel's information\n");
                            str = System.Console.ReadLine();
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
                            System.Console.WriteLine("select your choice:\n"+
                                                "stations - show BaseStations list \n"+
                                                "drones - show Drones list\n"+
                                                "customers - show Customers list\n"+
                                                "parcels - show Parcels list\n"+
                                                "unlinked - show unlinked parcels list\n"+
                                                "charge - show Basestatios with available charging slots list\n");
                            str = System.Console.ReadLine();
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
                    case "exit":
                        { break; }
                    default:
                        {
                            Console.WriteLine("Wrong selection, restart the process.\n");
                            break;
                        }

                }




            } while (str != "exit");

        }

        
        private static void inputBaseStation(BaseStation station)
        {
            int num;
            double x;
            Console.WriteLine("enter Base Station's id\n");
            if(int.TryParse(Console.ReadLine(), out num))
                station.Id = num;
            Console.WriteLine("enter Base Station's name\n");
            station.Name = Console.ReadLine();
            Console.WriteLine("enter Base Station's longtitude coordinate\n");
            if(double.TryParse(Console.ReadLine(), out x))
                station.Longitude = x;
            Console.WriteLine("enter Base Station's lattitude coordinate\n");
            if (double.TryParse(Console.ReadLine(), out x))
                station.Lattitude = x;
            Console.WriteLine("enter Base Station's number of charging slots\n");
            if (int.TryParse(Console.ReadLine(), out num))
                station.NumOfSlots = num;
        }
        private static void inputDrone(Drone dr)
        {
            int num;
            double x;
            Console.WriteLine("Enter Drone's id\n");
            if(int.TryParse(Console.ReadLine(), out num))
                dr.Id = num;
            Console.WriteLine("Enter Drone's model\n");
            dr.Model = Console.ReadLine();
            Console.WriteLine(@"Enter Drone's Max Weight category\n 
                                1: light   2: medium   3: heavy \n");
            if(int.TryParse(Console.ReadLine(), out num))
                dr.MaxWeight = (WeightCategories)num;
            Console.WriteLine(@"Enter Drone's Status\n 
                                1: available   2: maintenance   3: delivery \n");
            if(int.TryParse(Console.ReadLine(), out num))
                dr.Status = (DroneStatus)num;
            Console.WriteLine("Enter Drone's battery level\n");
            if(double.TryParse(Console.ReadLine(), out x))
                dr.Battery = x;   
        }
        private static void inputCustomer(Customer person)
        {
            int num;
            double x;
            Console.WriteLine("enter Customer's id\n");
            if (int.TryParse(Console.ReadLine(), out num))
                person.Id = num;
            Console.WriteLine("Enter Customer's name\n");
            person.Name = Console.ReadLine();
            Console.WriteLine("Enter Customers's Phone Number\n");
            person.Name = Console.ReadLine();
            Console.WriteLine("Enter Base Station's longtitude coordinate\n");
            if (double.TryParse(Console.ReadLine(), out x))
                person.Longitude = x;
            Console.WriteLine("Enter Base Station's lattitude coordinate\n");
            if (double.TryParse(Console.ReadLine(), out x))
                person.Lattitude = x;
        }
        private static void inputParcel(Parcel package)
        {
            int num;
        
            
               
            Console.WriteLine("Enter Parcel's sender id\n");
            if (int.TryParse(Console.ReadLine(), out num))
                package.SenderId = num;
            Console.WriteLine("Enter Parcel's target id\n");
            if (int.TryParse(Console.ReadLine(), out num))
                package.TargetId = num;
            Console.WriteLine(@"Enter Parcel's Weight category\n 
                                1: light   2: medium   3: heavy \n");
            if (int.TryParse(Console.ReadLine(), out num))
               package.Weight = (WeightCategories)num;
            package.Scheduled = DateTime.Now;
        }
        private static void printBaseStations(List<BaseStation> stations)
        {
            foreach (BaseStation stn in stations)
            {
                Console.WriteLine(stn.ToString());
            }
        }
        public static void printDrones(List<Drone> drns)
        {
            foreach (Drone dr in drns)
            {
                Console.WriteLine(dr.ToString());
            }
        }
        public static void printCustomers(List<Customer> customers)
        {
            foreach (Customer cst in customers)
            {
                Console.WriteLine(cst.ToString());
            }
        }
        public static void printParcels(List<Parcel> parcels)
        {
            foreach (Parcel prcl in parcels)
            {
                Console.WriteLine(prcl.ToString());
            }
        }
       

    }
}

