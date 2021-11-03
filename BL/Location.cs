


namespace IBL.BO
{
    public class Location
    {
        public double Longtitude { get; set; }
        public double Lattitude { get; set; }
        public override string ToString()
        {
            string str = "";
            str += "Longitude is " + StringAdapter.LongtitudeToDMS(Longtitude) + "\n";
            str += "Latitude is " + StringAdapter.LattitudeToDMS(Lattitude) + "\n";
            return str;
        }
    }
}