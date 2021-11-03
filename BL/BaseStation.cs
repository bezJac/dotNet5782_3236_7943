namespace IBL.BO
{
        public class BaseStation
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
                result += "Longitude is " + Longitude + "\n";
                result += "Latitude is " + Lattitude + "\n";
                result += $"NumOfSlots is {NumOfSlots}\n";

                return result;
            }

        }
    
}