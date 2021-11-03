namespace IBL.BO
{
    
        public class Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatus Status { get; set; }
            public int Battery { get; set; }
            public Delivery Parcel { get; set; }
            public Location Location { get; set; }
            public override string ToString()
            {
                string result = "";
                result += $"Id is { Id }\n";
                result += $"Model is { Model }\n";
                result += $"MaxWeight is { MaxWeight }\n";
                result += $"Status is { Status }\n";
                result += $"Battery level is { Battery }\n";
                return result;
            }
        }

    
}