using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    
    public class ParcelInDelivery
    {
        public int Id { get; set; }
        public bool InTransit { get; set; }
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
