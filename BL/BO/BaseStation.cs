using System.Collections.Generic;
using System.Linq;

namespace BO
{
    /// <summary>
    /// BL Base Station  - object contains base station details
    /// </summary>
    public class BaseStation
    {
        /// <summary>
        /// base station identification number
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// name of base station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// location of base station
        /// </summary>
        public Location StationLocation { get; set; }
        /// <summary>
        /// current number of available charging slots at station 
        /// </summary>
        public int? NumOfSlots { get; set; }
        /// <summary>
        /// list of all drones currentlly charging at station
        /// </summary>
        public IEnumerable<DroneCharge> DronesCharging { get; set; }
        public override string ToString()
        {
            int i = 1;
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Location:\n{ StationLocation }";
            result += $"Number of available charging slots: {NumOfSlots}\n";
            if (DronesCharging.Any())
            {
                result += $"List of Drones being charged at station:\n";
                foreach (DroneCharge dr in DronesCharging)
                {
                    result += $"------{i++}------\n {dr}";
                }
            }
            return result;
        }

    }

}