using System.Collections.Generic;
using System.Linq;

namespace IBL.BO
{
        public class BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Location StationLocation { get; set; }
            public int NumOfSlots { get; set; }
            public IEnumerable<DroneCharge> DronesCharging { get; set; }
            public override string ToString()
            {
                int i = 1;
                string result = "";
                result += $"Id: {Id}\n";
                result += $"Name: {Name}\n";
                result += $"Location: { StationLocation }\n";
                result += $"Number of available chatging slots: {NumOfSlots}\n";
                if (DronesCharging.Any())
                {
                    result += $"List of Drones being charged at station:\n";
                    foreach (DroneCharge dr in DronesCharging)
                    {
                        result += $"{i++}:\n {dr}";
                    }
                }
                return result;
            }

        }
    
}