using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// drone charge details for BaseStation object interaction
    /// </summary>
    public class DroneCharge
    {
        /// <summary>
        /// identification number  of drone
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// battery level of drone 
        /// </summary>
        public int Battery { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: { Id }\n";
            result += $"Battery level: { Battery }\n";
            return result;
        }
    }
}
