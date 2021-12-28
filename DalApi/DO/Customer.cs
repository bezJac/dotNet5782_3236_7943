using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//

namespace DO
{
    /// <summary>
    /// struct representing a customer's data
    /// </summary>
    public struct Customer
    {
        /// <summary>
        /// customer identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// customer full name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// customer phone number
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// lattitude coordinate of current customer location
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// longtitude coordinate of current customer location
        /// </summary>
        public double Lattitude { get; set; }

        public override string ToString()
        {
            string result = "";
            result += "Id is " + Id + "\n";
            result += "Name is " + Name + "\n";
            result += "Phone is " + Phone + "\n";
            result += "Longitude is " + Longitude + "\n";
            result += "Latitude is " + Lattitude + "\n";
            return result;
        }


    }
}


