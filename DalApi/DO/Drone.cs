using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DO
{
    /// <summary>
    /// struct representing a drone's data
    /// </summary>
    public struct Drone
    {
        /// <summary>
        /// drone identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// drone model name
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        ///  maximum weight category the drone can carry
        /// </summary>
        public WeightCategories MaxWeight { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Id is { Id }\n";
            result += $"Model is { Model }\n";
            result += $"MaxWeight is { MaxWeight }\n";
            return result;

        }
    }

}


