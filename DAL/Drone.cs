using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatus Status { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"Id is { Id }\n";
                result += $"Model is { Model }\n";
                result += $"MaxWeight is { MaxWeight }\n";
                result += $"Status is { Status }\n";
                result += $"Battery is { Battery }\n";
                return result;

            }
        }

    }
}

