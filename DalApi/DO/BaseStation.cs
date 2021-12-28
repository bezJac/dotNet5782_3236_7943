using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DO
{
    /// <summary>
    /// struct representing a base station's data
    /// </summary>
    public struct BaseStation
    {
        /// <summary>
        /// identification number of base station
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// base station name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// longtitude coordinate of base station current location
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// lattitude coordinate of base station current location
        /// </summary>
        public double Lattitude { get; set; }
        /// <summary>
        /// number of current available charging slots at base station
        /// </summary>
        public int NumOfSlots { get; set; }

        public override string ToString()
        {
            string result = "";
            result += $"Id is {Id}\n";
            result += $"Name is {Name}\n";
            result += "Longitude is " + Longitude + "\n";
            result += "Latitude is " + Lattitude + "\n";
            result += $"NumOfSlots is {NumOfSlots}\n";

            return result;
        }


    }
}


