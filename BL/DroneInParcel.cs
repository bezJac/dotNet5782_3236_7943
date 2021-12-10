using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public int Battery { get; set; }
        public Location DroneLocation { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: { Id }\n";
            result += $"Battery level: { Battery }\n";
            result += $"Location:\n {DroneLocation}";
            return result;
        }
    }
}
