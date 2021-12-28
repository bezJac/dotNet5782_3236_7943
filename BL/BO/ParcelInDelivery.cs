using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// details of parcel for Drone object interaction
    /// </summary>
    public class ParcelInDelivery
    {
        /// <summary>
        /// identification number of delivery
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// boolan indicator if parcel was picked up by drone
        /// </summary>
        public bool InTransit { get; set; }
        /// <summary>
        /// priority of delivery
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// weight ctegory of parcel
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// sending customer details
        /// </summary>
        public CustomerInParcel Sender { get; set; }
        /// <summary>
        /// recieving customer details
        /// </summary>
        public CustomerInParcel Target { get; set; }
        /// <summary>
        /// location objects with sending customer location details
        /// </summary>
        public Location SenderLocation { get; set; }
        /// <summary>
        /// location objects with recieving customer location details
        /// </summary>
        public Location TargetLocation { get; set; }
        /// <summary>
        /// total distance of delivery in KM
        /// </summary>
        public double DeliveryDistance { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"ID: {Id}\n";
            result += $"Delivery status: {InTransit}\n";
            result += $"Sending customer details:\n { Sender }";
            result += $"Target customer details:\n { Target }";
            result += $"Priority: { Priority }\n";
            result += $"Weight: { Weight }\n";
            result += $"Sending Customer location:\n {SenderLocation}";
            result += $"Target Customer location:\n {TargetLocation}";
            result += $"Distance of delivery: " + String.Format("{0:0.0}",DeliveryDistance)+ " KM\n";
            return result;

        }
    }
}
