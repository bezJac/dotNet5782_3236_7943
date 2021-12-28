


namespace BO
{
    /// <summary>
    /// location represented in DMS coordinates
    /// </summary>
    public class Location
    {
        /// <summary>
        /// longtitude coordinate of location
        /// </summary>
        public double? Longtitude { get; set; }
        /// <summary>
        /// lattitude coordinate of location
        /// </summary>
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