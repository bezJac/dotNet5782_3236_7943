using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Distance
    {
        public static double GetDistance(Location location1, Location location2)
        { 
            
            double R = 6371;                    // Radius of the earth in km
            double dLat = DegToRad(location2.Lattitude - location1.Lattitude);
            double dLon = DegToRad(location2.Longtitude - location1.Lattitude);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(DegToRad(location1.Lattitude)) * Math.Cos(DegToRad(location2.Lattitude)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;                  // Distance in km
            return Math.Abs(d);
        }

        private static double DegToRad(double num)
        {
            return num * (Math.PI / 180);
        }
    }
}
