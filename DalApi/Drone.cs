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

        public int Id { get; set; }
        public string Model { get; set; }
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


