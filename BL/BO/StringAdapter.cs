using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class converts longittude and lattitude coordinates to their DMS form
    /// </summary>
    internal class StringAdapter
    {
        /// <summary>
        /// convert a decimal point longtitude coordinate to its DMS form
        /// </summary>
        /// <param name="val"> coordinate in decimal point</param>
        /// <returns> a string with coordinate in DMS form </returns>
        public static String LongtitudeToDMS(double val)
        {
            int tmp = (int)val;
            int tmp2 = (int)((val % (int)val) * 60);
            double tmp3 = (((val % (int)val) * 60) % tmp2) * 60;
            string result = $"{ tmp}{((char)176).ToString()} {tmp2}\" " + String.Format("{0:0.000}", tmp3) + "' ";
            if (val > 0)
                return result + "E";
            else
                return result + "W";
        }

        /// <summary>
        /// convert a decimal point lattitude coordinate to its DMS form
        /// </summary>
        /// <param name="val"> coordinate in decimal point</param>
        /// <returns> a string with coordinate in DMS form </returns>
        public static String LattitudeToDMS(double val)
        {
            int tmp = (int)val;
            int tmp2 = (int)((val % (int)val) * 60);
            double tmp3 = (((val % (int)val) * 60) % tmp2) * 60;
            string result = $"{ tmp}{((char)176).ToString()} {tmp2}\" " + String.Format("{0:0.000}", tmp3) + "' ";
            if (val > 0)
                return result + "N";
            else
                return result + "S";
        }
    }
}
        

