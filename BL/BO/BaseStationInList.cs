using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// base stations details for list view
    /// </summary>
    public class BaseStationInList
    {
        /// <summary>
        /// base station identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of base station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// current available chargimg slots at base station
        /// </summary>
        public int AvailableSlots { get; set; }
        /// <summary>
        /// current cccupied charging slots at base station
        /// </summary>
        public int OccupiedSlots{ get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Name: {Name}\n";
            result += $"Num of Available Slots: { AvailableSlots }\n";
            result += $"Num of Occupied Slots: {OccupiedSlots}\n";
            return result;
        }
    }
}
