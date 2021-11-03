using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    internal class StringAdapter
    {

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
        public static double DegToRad(double num)
        {
            return num * (Math.PI / 180);
        }
    }
}
        

