namespace IBL.BO
{
    
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatus Status { get; set; }
            public int Battery { get; set; }
            public ParcelInDelivery Parcel { get; set; }
            public Location Location { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"ID: { Id }\n";
                result += $"Model: { Model }\n";
                result += $"MaxWeight: { MaxWeight }\n";
                result += $"Status: { Status }\n";
                result += $"Battery level: { Battery } %\n";
                if(Parcel!=null)
                    result += $"Parcel in delivery details are:\n {Parcel}";
                result += $"Drone location:\n{Location}";
                return result;
            }
        }

    
}