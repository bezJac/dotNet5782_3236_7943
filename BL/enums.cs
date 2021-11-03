using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public enum WeightCategories
        {
            Light = 1, Medium, Heavy
        }
        public enum Priority
        {
            Regular = 1, Express, Emergency
        }
        public enum DroneStatus
        {
            Available = 1, Maintenance, Delivery
        }
        public enum ParcelStatus
        {
            Orderd = 1, Linked, PickedUp, Delivered
        }
        

    }
}
