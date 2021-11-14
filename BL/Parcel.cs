using BL.BO;
using System;

namespace IBL.BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public DroneInParcel Drone { get; set; }
        public DateTime Ordered { get; set; }
        public DateTime Linked { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Sending customer details:\n { Sender }";
            result += $"Target customer details:\n { Target }";
            result += $"Weight: { Weight }\n";
            result += $"Priority: { Priority }\n";
            if(Drone!= null)
                result += $"Drone in parcel details:\n {Drone}";
            result += $"Parcel was ordered at: {Ordered}\n";
            result += $"Drone was linked to parcel at: {Ordered}\n";
            result += $"Drone picked up parcel from sender at {PickedUp}\n";
            result += $"Drone delievered parcel to target at {Delivered}\n";
            return result;

        }
    }

}