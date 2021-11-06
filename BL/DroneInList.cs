using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatus Status { get; set; }
        public int Battery { get; set; }
        public int ParcelId { get; set; }
        public Location DroneLocation { get; set; }
    }
}
