using BL.BO;
using System;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public Customer Sender { get; set; }
        public Customer Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public Drone Drone { get; set; }
        public DateTime Ordered { get; set; }
        public DateTime Linked { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            string result = "";
            result += "Id is " + Id + "\n";
            result += $"Sender is { Sender }\n";
            result += $"Target is { Target }\n";
            result += $"Weight is { Weight }\n";
            //result += $"Priority is { Priority }\n";
            //result += $"Requested is { Requested }\n";
            //result += $"DroneId is { DroneId }\n";
            //result += $"Scheduled is { Scheduled }\n";
            //result += $"PickedUp is { PickedUp }\n";
            //result += $"Delivered is { Delivered }\n";
            return result;

        }
    }

}