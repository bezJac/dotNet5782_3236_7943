﻿


namespace BO
{
    public class Location
    {
        public double? Longtitude { get; set; }
        public double? Lattitude { get; set; }
        public override string ToString()
        {
            string str = "";
            str += $"Longitude:  { StringAdapter.LongtitudeToDMS((double)Longtitude)}\n";
            str += $"Latitude: { StringAdapter.LattitudeToDMS((double)Lattitude)} \n";
            //string str = "";
            //str += $"Longitude:  { Longtitude}\n";
            //str += $"Latitude: { Lattitude} \n";
            return str;
        }
    }
}