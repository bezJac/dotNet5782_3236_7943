using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    
    public class Customer
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public Location location { get; set; }
        public IEnumerable<DeliveryAtCustomer> from { get; set; }
        public IEnumerable<DeliveryAtCustomer> to { get; set; }
        public override string ToString()
        {
            string result = "";
            result += $"Location is { location }\n";
                
            return result;
        }
    }

}
