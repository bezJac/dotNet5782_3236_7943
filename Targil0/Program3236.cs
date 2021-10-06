using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome3236();
            Welcome7943();
            Console.ReadKey();
        }
        static partial void Welcome7943();
        private static void Welcome3236()
        {
            Console.Write("Enter your name: ");
            String userName = Console.ReadLine();
            Console.WriteLine(userName + ", welcome to my first console application");
        }
    }
}