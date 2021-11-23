using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    
    public class ParcelInDelivery
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public Priority Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location SenderLocation { get; set; }
        public Location TargetLocation { get; set; }
        public double DeliveryDistance { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Id: {Id}\n";
            result += $"Delivery status: {Status}\n";
            result += $"Sending customer basic details:\n { Sender }";
            result += $"Target customer basic  details:\n { Target }";
            result += $"Priority: { Priority }\n";
            result += $"Weight: { Weight }\n";
            result += $"Sending Customer location:\n {SenderLocation}";
            result += $"Target Customer location:\n {TargetLocation}";
            result += $"Distance of delivery in KM:" + String.Format("{0:0.00}",DeliveryDistance)+"\n";
            return result;

        }
    }
}
