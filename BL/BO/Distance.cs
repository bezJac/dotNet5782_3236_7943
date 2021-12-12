using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Distance
    {
        public static double GetDistance(Location location1, Location location2)
        {

            var R = 6371;                    // Radius of the earth in km
            var dLat = DegToRad(location2.Lattitude - location1.Lattitude);
            var dLon = DegToRad(location2.Longtitude - location1.Longtitude);
            var a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2)) +
             (Math.Cos(DegToRad(location1.Lattitude)) * Math.Cos(DegToRad(location2.Lattitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2))
              ;
            //double b = a / (1 - a);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = (R) * c;                  // Distance in km
            return d;
            
            
        }

        private static double DegToRad(double num)
        {
            return num * (Math.PI / 180);
        }

       
        
    }
}
