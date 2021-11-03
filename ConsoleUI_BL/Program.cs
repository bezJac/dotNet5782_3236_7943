using System;
using IBL;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
            IBL.IBL data = new BL.BL();
            string str;
            int id1, id2;
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

                                        break;
                                    }
                                case "drone":
                                    {

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
    }
   
}
