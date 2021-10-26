using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int NumOfSlots { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"Id is {Id}\n";
                result += $"Name is {Name}\n";
                result += "Longitude is " + Conversions.LongtitudeToDMS(Longitude) + "\n";
                result += "Latitude is " + Conversions.LattitudeToDMS(Lattitude) + "\n";
                result += $"NumOfSlots is {NumOfSlots}\n";

                return result;
            }


        }
    }
    
}
