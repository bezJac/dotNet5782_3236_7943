using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.DO
{
    public class Distance
    {
        /// <summary>
        /// returns  distance from a point to a base station
        /// </summary>
        /// <param name="longt"> longtitude coordinate of point </param>
        /// <param name="latt"> lattitude coordinate of point </param>
        /// <param name="station"> station picked  to calculate distance to </param>
        /// <returns> double type - Distance in KM </returns>
        public static double GetDistance(double longt, double latt, BaseStation station)
        {
            return DistanceCalc(latt, longt, station.Lattitude, station.Longitude);
        }

        /// <summary>
        /// returns  distance from a point to a customer
        /// </summary>
        /// <param name="longt"> longtitude Coordinate of point </param>
        /// <param name="latt"> lattitude Coordinate of point </param>
        /// <param name="cstmr"> customer picked  to calculate distance to</param>
        /// <returns> double type - Distance in KM </returns>
        public static double GetDistance(double longt, double latt, Customer cstmr)
        {
            return DistanceCalc(latt, longt, cstmr.Lattitude, cstmr.Longitude);
        }

        /// <summary>
        /// calculates distance between two points 
        /// represented in longtitude and lattitude Coordinates
        /// </summary>
        /// <param name="lat1"> lattitude of point 1 </param>
        /// <param name="lon1"> Longtitude of point 1 </param>
        /// <param name="lat2"> lattitude of point 2 </param>
        /// <param name="lon2"> Longtitude of point 2 </param>
        /// <returns> Distance between points in KM </returns>
        private static double DistanceCalc(double lat1, double lon1, double lat2, double lon2)
        {
            // uses conversions class DegToRad function to convert results to radians
            double R = 6371;                    // Radius of the earth in km
            double dLat = DegToRad(lat2 - lat1);
            double dLon = DegToRad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;                  // Distance in km
            return d;
        }
        private static double DegToRad(double num)
        {
            return num * (Math.PI / 180);
        }
    }
}
