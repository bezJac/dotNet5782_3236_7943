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
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
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


