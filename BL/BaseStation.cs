using System.Collections.Generic;

namespace IBL.BO
{
        public class BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Location Location { get; set; }
            public int NumOfSlots { get; set; }
        public IEnumerable<DroneCharge> DronesCharging { get; set; }
        public override string ToString()
            {
                string result = "";
                result += $"Id is {Id}\n";
                result += $"Name is {Name}\n";
            result += $"Location is { Location }\n";
            result += $"NumOfSlots is {NumOfSlots}\n";
            foreach (DroneCharge dr in DronesCharging)
                result += dr.ToString();
                return result;
            }

        }
    
}