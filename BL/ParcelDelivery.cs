using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelDelivery
    {
        public int Id { get; set; }
        public Priority Priority { get; set; }
        public CustomerDelivery Sender { get; set; }
        public CustomerDelivery Target { get; set; }
    }
}
