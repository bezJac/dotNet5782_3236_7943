using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class manages distance calculations for BL
    /// </summary>
    internal class Distance
    {
        /// <summary>
        /// get the distance between two DMS coordinated locations in KM
        /// </summary>
        /// <param name="location1"> first location</param>
        /// <param name="location2"> second location</param>
        /// <returns> double type containing distance in KM </returns>
        public static double GetDistance(Location location1, Location location2)
        {

            var R = 6371;                    // Radius of the earth in km
            var dLat = DegToRad((double)location2.Lattitude - (double)location1.Lattitude);
            var dLon = DegToRad((double)location2.Longtitude - (double)location1.Longtitude);
            var a = (Math.Sin(dLat / 2) * Math.Sin(dLat / 2)) +
             (Math.Cos(DegToRad((double)location1.Lattitude)) * Math.Cos(DegToRad((double)location2.Lattitude)) *
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
