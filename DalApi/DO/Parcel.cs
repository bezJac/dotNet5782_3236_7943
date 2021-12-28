using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//

namespace DO
{
    /// <summary>
    /// struct representing a parcel for a delivery
    /// </summary>
    public struct Parcel
    {
        /// <summary>
        /// parcel identification number
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// identification number of sending customer
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// identification number of recieving customer
        /// </summary>
        public int TargetId { get; set; }
        /// <summary>
        /// weight category of parcel
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// priority of the delivery
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// date and time delivery was ordered
        /// </summary>
        public DateTime? Requested { get; set; }
        /// <summary>
        /// date and time parcel was linked to a drone
        /// </summary>
        public DateTime? Scheduled { get; set; }
        /// <summary>
        /// date and time parcel was picked up by drone for delivery
        /// </summary>
        public DateTime? PickedUp { get; set; }
        /// <summary>
        /// date and time parcel was delivered to target customer
        /// </summary>
        public DateTime? Delivered { get; set; }
        /// <summary>
        /// identification number of drone linked to parcel
        /// </summary>
        public int DroneId { get; set; }

        public override string ToString()
        {
            string result = "";
            result += "Id is " + Id + "\n";
            result += $"SenderId is { SenderId }\n";
            result += $"TargetId is { TargetId }\n";
            result += $"Weight is { Weight }\n";
            result += $"Priority is { Priority }\n";
            result += $"DroneId is { DroneId }\n";
            result += $"Requested is { Requested }\n";
            if (Scheduled != null)
                result += $"Scheduled is { Scheduled }\n";
            if (PickedUp != null)
                result += $"PickedUp is { PickedUp }\n";
            if (Delivered != null)
                result += $"Delivered is { Delivered }\n";
            return result;

        }
    }

}


