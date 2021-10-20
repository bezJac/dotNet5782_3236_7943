using System;
using DalObject;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            DalObject.DalObject data =  new DalObject.DalObject();
            string ch;
            do
            {
                System.Console.WriteLine(@"select your choice:\n
                                       a - add a new object\n
                                       b - update an object\n
                                       c - show details of object\n
                                       d - show details of array\n
                                       e - exit\n");
                ch = System.Console.ReadLine();


                switch (ch)
                {
                    case "a":
                        {
                            System.Console.WriteLine("select your choice:\n"
                                                        + "a - add a new Base Station\n"
                                                        + "b - add a new Drone\n"
                                                        + "c - add a new Customer\n"
                                                        + "d - add a new  Parcel\n");
                            ch = System.Console.ReadLine();
                            switch (ch)
                            {
                                case "a":
                                    {
                                        data.addBaseStation();
                                        break;
                                    }
                                case "b":
                                    {
                                        data.addDrone();
                                        break;
                                    }
                                case "c":
                                    {
                                        data.addCustomer();
                                        break;
                                    }
                                case "d":
                                    {
                                        data.addParcel();
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
                    case "b":
                        {
                            System.Console.WriteLine(@"select your choice:\n
                                                a - Link Parcel to Drone\n
                                                b - Pickup Parcel by Drone\n
                                                c - Deliver Parcel to Customer\n
                                                d - Send Drone to charging station\n
                                                e - Release Drone from charging station\n");
                            ch = System.Console.ReadLine();
                            switch (ch)
                            {
                                case "a":
                                    {
                                        data.linkParcelToDrone();
                                        break;
                                    }
                                case "b":
                                    {
                                        data.droneParcelPickup();
                                        break;
                                    }
                                case "c":
                                    {
                                        data.parcelDelivery();
                                        break;
                                    }
                                case "d":
                                    {
                                        data.chargeDrone();
                                        break;
                                    }
                                case "e":
                                    {
                                        data.releaseDroneCharge();
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
                    case "c":
                        {
                            System.Console.WriteLine(@"select your choice:\n
                                a - show a BaseStation's information\n
                                b - show a Drone's information\n
                                c - show a Customer's information\n
                                d - show a Parcel's information\n");
                            ch = System.Console.ReadLine();
                            switch (ch)
                            {
                                case "a":
                                    {
                                        data.printBaseStation();
                                        break;
                                    }
                                case "b":
                                    {
                                        data.printDrone();
                                        break;
                                    }
                                case "c":
                                    {
                                        data.printCustomer();
                                        break;
                                    }
                                case "d":
                                    {
                                        data.printParcel();
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
                    case "d":
                        {
                            System.Console.WriteLine(@"select your choice:\n
                                                a - show BaseStations list \n
                                                b - show Drones list\n
                                                c - show Customers list\n
                                                d - show Parcels list\n
                                                e - show unlinked parcels list\n
                                                f - show Basestatios with available charging slots list\n");
                            ch = System.Console.ReadLine();
                            switch (ch)
                            {
                                case "a":
                                    {
                                        data.printAllBaseStations();
                                        break;
                                    }
                                case "b":
                                    {
                                        data.printAllDrones();
                                        break;
                                    }
                                case "c":
                                    {
                                        data.printAllCustomers();
                                        break;
                                    }
                                case "d":
                                    {
                                        data.printAllParcels();
                                        break;
                                    }
                                case "e":
                                    {
                                        data.printUnlinkedParcels();
                                        break;
                                    }
                                case "f":
                                    {
                                        data.printAvailableCharge();
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
                    case "e":
                    { break; }
                    default:
                        {
                            Console.WriteLine("Wrong selection, restart the process.\n");
                            break;
                        }

                }




            } while (ch != "e");

        }
    }
}
