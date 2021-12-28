using System;

namespace BO
{
    /// <summary>
    /// BL Parcel - object contains a delivery details
    /// </summary>
    public class Parcel
    {
        /// <summary>
        /// parcel identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// sending customer details
        /// </summary>
        public CustomerInParcel Sender { get; set; }
        /// <summary>
        /// recieving customer details
        /// </summary>
        public CustomerInParcel Target { get; set; }
        /// <summary>
        /// weight category of parcel
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// priority of delivery
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// details of drone executing the delivery
        /// </summary>
        public DroneInParcel Drone { get; set; }
        /// <summary>
        /// date and time delivery was ordered
        /// </summary>
        public DateTime? Ordered { get; set; }
        /// <summary>
        /// date and time delivery was linked to a drone for pick up
        /// </summary>
        public DateTime? Linked { get; set; }
        /// <summary>
        /// date and time parcel was picked up by drone for delivery
        /// </summary>
        public DateTime? PickedUp { get; set; }
        /// <summary>
        /// date and time parcel was delivered by drone
        /// </summary>
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