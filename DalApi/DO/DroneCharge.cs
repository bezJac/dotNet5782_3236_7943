using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//

namespace DO
{
    /// <summary>
    /// struct containing data of a drone charging at station
    /// </summary>
    public struct DroneCharge
    {
        /// <summary>
        /// identification number of the drone 
        /// </summary>
        public int DroneId { get; set; }
        /// <summary>
        /// identification number of the station
        /// </summary>
        public int StationId { get; set; }
        /// <summary>
        /// date and time that drone began charging proccess
        /// </summary>
        public DateTime? EntranceTime { get; set; }
        public int BatteryAtEntrance { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"DroneId is { DroneId }\n";
            result += $"StationId is { StationId }\n";
            result += $"Entrance time is { EntranceTime }\n";
            return result;

        }
    }

}


