using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//
namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// struct representing a parcel
        /// </summary>
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DateTime? Requested { get; set; }
            public DateTime? Scheduled { get; set; }
            public DateTime? PickedUp { get; set; }
            public DateTime? Delivered { get; set; }
            public int DroneId { get; set; }

            public override string ToString()
            {
                string result = "";
                result += "Id is "+Id +"\n";
                result += $"SenderId is { SenderId }\n";
                result += $"TargetId is { TargetId }\n";
                result += $"Weight is { Weight }\n";
                result += $"Priority is { Priority }\n";
                result += $"DroneId is { DroneId }\n";
                result += $"Requested is { Requested }\n";
                if(Scheduled!=null)
                result += $"Scheduled is { Scheduled }\n";
                if(PickedUp!=null)
                result += $"PickedUp is { PickedUp }\n";
                if(Delivered!=null)
                result += $"Delivered is { Delivered }\n";
                return result;

            }
        }

    }
}

