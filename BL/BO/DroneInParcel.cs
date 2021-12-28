using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// drone details for Parcel object interaction
    /// </summary>
    public class DroneInParcel
    {
        /// <summary>
        /// drone identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// drone current battery level
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// current location of drone
        /// </summary>
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
