using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories? MaxWeight { get; set; }
        public DroneStatus? Status { get; set; }
        public int Battery { get; set; }
        public int? ParcelId { get; set; }
        public Location DroneLocation { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"ID: { Id }\n";
            result += $"Model: { Model }\n";
            result += $"MaxWeight: { MaxWeight }\n";
            result += $"Status: { Status }\n";
            result += $"Battery level: { Battery } %\n";
            if(ParcelId!=0)
            result += $"Parcel in delivery ID: {ParcelId}\n";
            result += $"Location:\n{DroneLocation}";
            return result;
        }

    }
}
