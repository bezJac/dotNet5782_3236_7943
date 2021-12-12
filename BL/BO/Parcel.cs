using System;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Weight { get; set; }
        public Priority Priority { get; set; }
        public DroneInParcel Drone { get; set; }
        public DateTime? Ordered { get; set; }
        public DateTime? Linked { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Sending customer details:\n { Sender }";
            result += $"Target customer details:\n { Target }";
            result += $"Weight: { Weight }\n";
            result += $"Priority: { Priority }\n";
            result += $"Parcel was ordered at: {Ordered}\n";
            if (Linked!=null)
                 result += $"Drone was linked to parcel at: {Linked}\n";
            if(PickedUp!=null)
                 result += $"Drone picked up parcel from sender at {PickedUp}\n";
            if(Delivered!=null)
                 result += $"Drone delievered parcel to target at {Delivered}\n";
            if (Drone != null)
                result += $"Drone in parcel details:\n {Drone}";
            return result;

        }
    }

}